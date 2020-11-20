using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Zoka.SwitchableForms
{
	/// <summary>Extensions method to configure the usage of the SwitchableForms</summary>
	public static class SwitchableFormConfigure 
	{
		/// <summary>Will configure the SwitchableForms component for usage within web project</summary>
		public static IMvcBuilder							ConfigureSwitchableForms(this IMvcBuilder _mvc_builder)
		{
			if (_mvc_builder == null)
				throw new ArgumentNullException(nameof(_mvc_builder));
			_mvc_builder.AddMvcOptions(o => {
				o.ModelBinderProviders.Insert(0, new SwitchableFormsBinderProvider());
			});
			return _mvc_builder;
		}
	}
}
