using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;

namespace Zoka.SwitchableForms
{
	/// <inheritdoc />
	public class SwitchableFormsBinderProvider : IModelBinderProvider
	{
		/// <inheritdoc />
		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			if (typeof(SwitchableFormsModel).IsAssignableFrom(context.Metadata.ModelType))
			{
				// first check that it is SwitchableForms<...> type - must not be overriden
				var switchable_form_model_binders = new List<(Type, ModelMetadata, IModelBinder)>();
				if (context.Metadata.ModelType.IsGenericType &&
					(
						(context.Metadata.ModelType.GenericTypeArguments.Length == 2 && context.Metadata.ModelType.GetGenericTypeDefinition() == typeof(SwitchableForms<,>)) ||
						(context.Metadata.ModelType.GenericTypeArguments.Length == 3 && context.Metadata.ModelType.GetGenericTypeDefinition() == typeof(SwitchableForms<,,>)) ||
						(context.Metadata.ModelType.GenericTypeArguments.Length == 4 && context.Metadata.ModelType.GetGenericTypeDefinition() == typeof(SwitchableForms<,,,>)) ||
						(context.Metadata.ModelType.GenericTypeArguments.Length == 5 && context.Metadata.ModelType.GetGenericTypeDefinition() == typeof(SwitchableForms<,,,,>)) ||
						(context.Metadata.ModelType.GenericTypeArguments.Length == 6 && context.Metadata.ModelType.GetGenericTypeDefinition() == typeof(SwitchableForms<,,,,,>)) ||
						(context.Metadata.ModelType.GenericTypeArguments.Length == 7 && context.Metadata.ModelType.GetGenericTypeDefinition() == typeof(SwitchableForms<,,,,,,>)) 
					))
				{
					// and store binder and metadata for every single type
					var switchable_form_model_types = context.Metadata.ModelType.GenericTypeArguments;
					foreach (var switchable_form_model_type in switchable_form_model_types)
					{
						var model_metadata = context.MetadataProvider.GetMetadataForType(switchable_form_model_type);
						switchable_form_model_binders.Add((switchable_form_model_type, model_metadata, context.CreateBinder(model_metadata)));
					}
				}
				else
				{
					throw new InvalidOperationException("SwitchableForms<> type cannot be overriden by user");
				}

				// now we need to create binder for the SwitchableForms
				// it is ComplexTypeModelBinder, which requires list of all binders for all its properties
				var switchable_forms_binder = new Dictionary<ModelMetadata, IModelBinder>();
				for (int i = 0; i < context.Metadata.Properties.Count; i++)
				{
					var switchable_forms_property = context.Metadata.Properties[i];
					IModelBinder switchable_forms_property_binder;
					if(switchable_forms_property.ModelType == typeof(List<SwitchableFormsModel.SwitchableFormModelWrapper>))
					{
						var switchable_form_wrapper_property_binders = new Dictionary<ModelMetadata, IModelBinder>();
						var switchable_form_wrapper_metadata = context.MetadataProvider.GetMetadataForType(typeof(SwitchableFormsModel.SwitchableFormModelWrapper));
						for (int j = 0; j < switchable_form_wrapper_metadata.Properties.Count; j++)
						{
							var switchable_form_wrapper_property = switchable_form_wrapper_metadata.Properties[j];
							IModelBinder switchable_form_wrapper_property_binder;
							if (switchable_form_wrapper_property.ModelType == typeof(ISwitchableFormModel))
							{
								switchable_form_wrapper_property_binder = new ISwitchableFormModelBinder(switchable_form_model_binders);
							}
							else
							{
								switchable_form_wrapper_property_binder = context.CreateBinder(switchable_form_wrapper_property);
							}
							switchable_form_wrapper_property_binders.Add(switchable_form_wrapper_property, switchable_form_wrapper_property_binder);
						}
						var switchable_form_wrapper_binder = new ComplexTypeModelBinder(switchable_form_wrapper_property_binders, context.Services.GetService(typeof(ILoggerFactory)) as ILoggerFactory);
						switchable_forms_property_binder = new CollectionModelBinder<SwitchableFormsModel.SwitchableFormModelWrapper>(switchable_form_wrapper_binder, context.Services.GetService(typeof(ILoggerFactory)) as ILoggerFactory);
					}
					else
						switchable_forms_property_binder = context.CreateBinder(switchable_forms_property);
					switchable_forms_binder.Add(switchable_forms_property, switchable_forms_property_binder);
				}
				return new ComplexTypeModelBinder(switchable_forms_binder, context.Services.GetService(typeof(ILoggerFactory)) as ILoggerFactory);
			}

			return null;
		}
	}
}
