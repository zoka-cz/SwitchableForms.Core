/**********************************************************************************************************************
 * 
 *	made by: Michal Linhart ("Zoka")
 *  contact: michal.linhart@gmail.com
 *
 *	Usage:
 *		$("#SwitchableForms_ModelId").SwitchableForms(options)
 *
 *	Where:
 *		options is object with following members:
 *			initialSwitcherValue - the value of the switcher
 *
 **********************************************************************************************************************/


(function ($) {

	$.SwitchableForms = function (element, options) {

		var defaults = {
		}

		var plugin = this;

		plugin.settings = {};

		var element = element;
		var $element = $(element);

        //var $switcher = $(element).children("div.Switcher").find("input"); // second find is safe

		var $switcher = $(element).find("input[type=\"radio\"]");

		plugin.init = function () {
			plugin.settings = $.extend({}, defaults, options);

			$switcher.on("change", function () {
				var $editors = $(element).children("div.Editors");
				var selected_form_div = $editors.children("div.SwitchableFormId" + $(this).val());
				selected_form_div.removeClass("hidden");
				selected_form_div.siblings().hide();
				selected_form_div.show();
			});

			$(element).children("div.Editors").children("div.SwitchableForm").hide();
			$(element).children("div.Editors").children("div.SwitchableFormId" + plugin.settings.initialSwitcherValue).removeClass("hidden").show();
		}

		plugin.init();
	}

	$.fn.SwitchableForms = function (options) {

		return this.each(function () {
			if (undefined == $(this).data('SwitchableForms')) {
				var plugin = new $.SwitchableForms(this, options);
				$(this).data('SwitchableForms', plugin);
			}
		});

	}

})(jQuery);