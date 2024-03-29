﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Zoka.SwitchableForms
{

	/// <summary>Interface which must be implemented by every switchable form</summary>
	public interface ISwitchableFormModel
	{

	}

	/// <summary>Abstract class, used for accessing switchable models</summary>
	public abstract class SwitchableFormsModel
	{
		/// <summary>Holder of form model object</summary>
		public class SwitchableFormModelWrapper : IValidatableObject
		{
			/// <summary>The id of the model, must be unique amongst other SwitchableFormModel</summary>
			[HiddenInput(DisplayValue = false)]
			public int										ModelId { get; set; }

			/// <summary>The name of the model to display</summary>
			[HiddenInput(DisplayValue = false)]
			public string									ModelDisplayName { get; set; }

			/// <summary>The single form model</summary>
			public ISwitchableFormModel						Model { get; set; }

			/// <summary>Empty constructor</summary>
			public SwitchableFormModelWrapper() {}

			/// <summary>Constructor</summary>
			public SwitchableFormModelWrapper(int _model_id, string _model_display_name, ISwitchableFormModel _model)
			{
				ModelId = _model_id;
				ModelDisplayName = _model_display_name;
				Model = _model;
			}

			/// <summary>Validation</summary>
			/// <remarks>
			///		The IValidatableObject is implemented only to ensure, that all form models are correctly validated.
			///		Possible bug in ASP.NET Core. See https://github.com/dotnet/aspnetcore/issues/27882 for more details.
			/// </remarks>
			public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
			{
				yield return ValidationResult.Success;
			}
		}

		/// <summary>Switcher to switch between form models</summary>
		[Required]
		public int?											Switcher { get; set; }

		/// <summary>List of models</summary>
		protected List<SwitchableFormModelWrapper>			m_Models = new List<SwitchableFormModelWrapper>();

		/// <summary>List of models</summary>
		public List<SwitchableFormModelWrapper>				Models
		{
			get { return m_Models; }
		}

		/// <summary>Will return the selected model</summary>
		public ISwitchableFormModel							GetSelectedModel()
		{
			foreach (var model in Models)
			{
				if (model.ModelId.Equals(Switcher))
					return model.Model;
			}
			return null;
		}

		/// <summary>Will return the selected model typed or null if the model is not selected or not the desired type</summary>
		public T											GetSelectedModel<T>()
			where T : class, ISwitchableFormModel
		{
			var model = GetSelectedModel();
			return model as T;
		}

	}

	/// <summary>The class which holds two form models of specific types, and when rendered it allows user to select one of them.</summary>
	public class SwitchableForms<T1, T2> : SwitchableFormsModel
		where T1 : ISwitchableFormModel
		where T2 : ISwitchableFormModel
	{
		/// <summary>Constructor</summary>
		public SwitchableForms(
			int _model1_id, string _model1_display_name, T1 _model1,
			int _model2_id, string _model2_display_name, T2 _model2)
			: base()
		{
			m_Models.Add(new SwitchableFormModelWrapper(_model1_id, _model1_display_name, _model1));
			m_Models.Add(new SwitchableFormModelWrapper(_model2_id, _model2_display_name, _model2));
		}

	}

	/// <summary>The class which holds three form models of specific types, and when rendered it allows user to select one of them.</summary>
	public class SwitchableForms<T1, T2, T3> : SwitchableForms<T1, T2>
		where T1 : ISwitchableFormModel
		where T2 : ISwitchableFormModel
		where T3 : ISwitchableFormModel
	{
		/// <summary>Constructor</summary>
		public SwitchableForms(
			int _model1_id, string _model1_display_name, T1 _model1,
			int _model2_id, string _model2_display_name, T2 _model2,
			int _model3_id, string _model3_display_name, T3 _model3)
			: base(_model1_id, _model1_display_name, _model1, _model2_id, _model2_display_name, _model2)
		{
			m_Models.Add(new SwitchableFormModelWrapper(_model3_id, _model3_display_name, _model3));
		}
	}

	/// <summary>The class which holds four form models of specific types, and when rendered it allows user to select one of them.</summary>
	public class SwitchableForms<T1, T2, T3, T4> : SwitchableForms<T1, T2, T3>
		where T1 : ISwitchableFormModel
		where T2 : ISwitchableFormModel
		where T3 : ISwitchableFormModel
		where T4 : ISwitchableFormModel
	{
		/// <summary>Constructor</summary>
		public SwitchableForms(
			int _model1_id, string _model1_display_name, T1 _model1,
			int _model2_id, string _model2_display_name, T2 _model2,
			int _model3_id, string _model3_display_name, T3 _model3,
			int _model4_id, string _model4_display_name, T4 _model4)
			: base(
				_model1_id, _model1_display_name, _model1, 
				_model2_id, _model2_display_name, _model2,
				_model3_id, _model3_display_name, _model3)
		{
			m_Models.Add(new SwitchableFormModelWrapper(_model4_id, _model4_display_name, _model4));
		}
	}

	/// <summary>The class which holds five form models of specific types, and when rendered it allows user to select one of them.</summary>
	public class SwitchableForms<T1, T2, T3, T4, T5> : SwitchableForms<T1, T2, T3, T4>
		where T1 : ISwitchableFormModel
		where T2 : ISwitchableFormModel
		where T3 : ISwitchableFormModel
		where T4 : ISwitchableFormModel
		where T5 : ISwitchableFormModel
	{
		/// <summary>Constructor</summary>
		public SwitchableForms(
			int _model1_id, string _model1_display_name, T1 _model1,
			int _model2_id, string _model2_display_name, T2 _model2,
			int _model3_id, string _model3_display_name, T3 _model3,
			int _model4_id, string _model4_display_name, T4 _model4,
			int _model5_id, string _model5_display_name, T5 _model5)
			: base(
				_model1_id, _model1_display_name, _model1,
				_model2_id, _model2_display_name, _model2,
				_model3_id, _model3_display_name, _model3,
				_model4_id, _model4_display_name, _model4)
		{
			m_Models.Add(new SwitchableFormModelWrapper(_model5_id, _model5_display_name, _model5));
		}
	}

	/// <summary>The class which holds six form models of specific types, and when rendered it allows user to select one of them.</summary>
	public class SwitchableForms<T1, T2, T3, T4, T5, T6> : SwitchableForms<T1, T2, T3, T4, T5>
		where T1 : ISwitchableFormModel
		where T2 : ISwitchableFormModel
		where T3 : ISwitchableFormModel
		where T4 : ISwitchableFormModel
		where T5 : ISwitchableFormModel
		where T6 : ISwitchableFormModel
	{
		/// <summary>Constructor</summary>
		public SwitchableForms(
			int _model1_id, string _model1_display_name, T1 _model1,
			int _model2_id, string _model2_display_name, T2 _model2,
			int _model3_id, string _model3_display_name, T3 _model3,
			int _model4_id, string _model4_display_name, T4 _model4,
			int _model5_id, string _model5_display_name, T5 _model5,
			int _model6_id, string _model6_display_name, T6 _model6)
			: base(
				_model1_id, _model1_display_name, _model1,
				_model2_id, _model2_display_name, _model2,
				_model3_id, _model3_display_name, _model3,
				_model4_id, _model4_display_name, _model4,
				_model5_id, _model5_display_name, _model5)
		{
			m_Models.Add(new SwitchableFormModelWrapper(_model6_id, _model6_display_name, _model6));
		}
	}

	/// <summary>The class which holds seven form models of specific types, and when rendered it allows user to select one of them.</summary>
	public class SwitchableForms<T1, T2, T3, T4, T5, T6, T7> : SwitchableForms<T1, T2, T3, T4, T5, T6>
		where T1 : ISwitchableFormModel
		where T2 : ISwitchableFormModel
		where T3 : ISwitchableFormModel
		where T4 : ISwitchableFormModel
		where T5 : ISwitchableFormModel
		where T6 : ISwitchableFormModel
		where T7 : ISwitchableFormModel
	{
		/// <summary>Constructor</summary>
		public SwitchableForms(
			int _model1_id, string _model1_display_name, T1 _model1,
			int _model2_id, string _model2_display_name, T2 _model2,
			int _model3_id, string _model3_display_name, T3 _model3,
			int _model4_id, string _model4_display_name, T4 _model4,
			int _model5_id, string _model5_display_name, T5 _model5,
			int _model6_id, string _model6_display_name, T6 _model6,
			int _model7_id, string _model7_display_name, T7 _model7)
			: base(
				_model1_id, _model1_display_name, _model1,
				_model2_id, _model2_display_name, _model2,
				_model3_id, _model3_display_name, _model3,
				_model4_id, _model4_display_name, _model4,
				_model5_id, _model5_display_name, _model5,
				_model6_id, _model6_display_name, _model6)
		{
			m_Models.Add(new SwitchableFormModelWrapper(_model7_id, _model7_display_name, _model7));
		}
	}

}
