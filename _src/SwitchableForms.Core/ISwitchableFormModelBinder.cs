using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Zoka.SwitchableForms
{
	/// <summary>
	/// 
	/// </summary>
	public class ISwitchableFormModelBinder : IModelBinder
	{
		private readonly List<(Type, ModelMetadata, IModelBinder)> m_SwitchableFormModelBinders;

		/// <summary>Constructor</summary>
		public ISwitchableFormModelBinder(List<(Type, ModelMetadata, IModelBinder)> _switchable_form_model_binders)
		{
			m_SwitchableFormModelBinders = _switchable_form_model_binders;
		}

		/// <inheritdoc />
		public async Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext.ModelName.EndsWith("].Model"))
			{
				var indexer_idx = bindingContext.ModelName.LastIndexOf('[');
				string idxs = "";
				while (++indexer_idx < bindingContext.ModelName.Length && bindingContext.ModelName[indexer_idx] != ']')
				{
					idxs += bindingContext.ModelName[indexer_idx];
				}
				int idx;
				if (!int.TryParse(idxs, out idx))
				{
					bindingContext.Result = ModelBindingResult.Failed();
					return;
				}

				if (idx < 0 || idx >= m_SwitchableFormModelBinders.Count)
				{
					bindingContext.Result = ModelBindingResult.Failed();
					return;
				}

				var newBindingContext = DefaultModelBindingContext.CreateBindingContext(
					bindingContext.ActionContext,
					bindingContext.ValueProvider,
					m_SwitchableFormModelBinders[idx].Item2,
					bindingInfo: null,
					bindingContext.ModelName);

				await m_SwitchableFormModelBinders[idx].Item3.BindModelAsync(newBindingContext);
				bindingContext.Result = newBindingContext.Result;

				// find out whether this particular model has been selected by the user, and if not,
				// disable the validation for him by not passing ValidationStateEntry.
				// Validatior will then think it is only ISwitchableFormModel and will say,
				// that the model is Valid.
				var disable_validation = false;
				var prefix_end_idx = bindingContext.ModelName.LastIndexOf($"Models[{idx}].Model");
				if (prefix_end_idx != -1)
				{
					// selected model id
					var prefix = bindingContext.ModelName.Substring(0, prefix_end_idx);
					if (prefix.EndsWith("."))
						prefix = prefix.Substring(0, prefix.Length - 1);
					var switcher_key = ModelNames.CreatePropertyModelName(prefix, "Switcher");
					var switcher_val = bindingContext.ValueProvider.GetValue(switcher_key);

					// current model id
					prefix_end_idx = bindingContext.ModelName.LastIndexOf(".Model");
					prefix = bindingContext.ModelName.Substring(0, prefix_end_idx);
					var model_switcher_value_key = ModelNames.CreatePropertyModelName(prefix, "ModelId");
					var model_switcher_value_key_val = bindingContext.ValueProvider.GetValue(model_switcher_value_key);

					if (switcher_val.Length == 1 && model_switcher_value_key_val.Length == 1 && model_switcher_value_key_val.FirstValue != switcher_val.FirstValue)
						disable_validation = true;
				}
				if (!disable_validation)
				{
					bindingContext.ValidationState.Add(bindingContext.Result.Model, new ValidationStateEntry
					{
						Metadata = m_SwitchableFormModelBinders[idx].Item2,
					});
				}

				return;
			}

			bindingContext.Result = ModelBindingResult.Failed();
			return;
		}
	}
}
