using System;
using System.Windows.Forms;
using PlaneAlerter.Enums;
using PlaneAlerter.Models;
using PlaneAlerter.Services;

namespace PlaneAlerter.Forms {
	/// <summary>
	/// Form for changing email content config
	/// </summary>
	internal partial class EmailContentConfigForm :Form {
		private readonly ISettingsManagerService _settingsManagerService;

		/// <summary>
		/// Constructor
		/// </summary>
		public EmailContentConfigForm(ISettingsManagerService settingsManagerService) {
			_settingsManagerService = settingsManagerService;

			//Initialise form components
			InitializeComponent();
			Theme.Apply(this);
			Theme.MakePrimary(saveButton);

			//Update form elements with current settings
			var config = _settingsManagerService.EmailContentConfig;
			receiverNameCheckBox.Checked = config.ReceiverName;
			transponderTypeCheckBox.Checked = config.TransponderType;
			radarLinkCheckBox.Checked = config.RadarLink;
			afLookupCheckBox.Checked = config.AfLookup;
			mapCheckBox.Checked = config.Map;
			photosCheckBox.Checked = config.AircraftPhotos;
			reportCheckBox.Checked = config.ReportLink;
			kmlCheckbox.Checked = config.KmlFile;

			plAll.Checked = config.PropertyList == PropertyListType.All;
			plEssentials.Checked = config.PropertyList == PropertyListType.Essentials;
			plHidden.Checked = config.PropertyList == PropertyListType.Hidden;
		}

		/// <summary>
		/// Save button click
		/// </summary>
		/// <param name="sender">Sender</param>
		/// <param name="e">Event Args</param>
		private void saveButton_Click(object sender, EventArgs e)
		{
			PropertyListType propertyList;

			if (plAll.Checked)
				propertyList = PropertyListType.All;
			else if (plEssentials.Checked)
				propertyList = PropertyListType.Essentials;
			else
				propertyList = PropertyListType.Hidden;

			//Save settings to current settings
			_settingsManagerService.EmailContentConfig = new EmailContentConfig(
				afLookupCheckBox.Checked,
				photosCheckBox.Checked,
				mapCheckBox.Checked,
				kmlCheckbox.Checked,
				propertyList,
				radarLinkCheckBox.Checked,
				reportCheckBox.Checked,
				receiverNameCheckBox.Checked,
				transponderTypeCheckBox.Checked
			);
			_settingsManagerService.Save();

			//Close form
			DialogResult = DialogResult.OK;
			Close();
		}
	}
}
