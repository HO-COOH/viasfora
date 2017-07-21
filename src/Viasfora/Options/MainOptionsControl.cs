﻿using System;
using System.Windows.Forms;
using Winterdom.Viasfora.Settings;
using Winterdom.Viasfora.Rainbow;
using Winterdom.Viasfora.Xml;
using Winterdom.Viasfora.Contracts;
using Winterdom.Viasfora.Classifications;
using Winterdom.Viasfora.Rainbow.Classifications;

namespace Winterdom.Viasfora.Options {
  public partial class MainOptionsControl : UserControl {
    const String XML_FILTER = "XML Files (*.xml)|*.xml";
    const String THEME_FILTER = "Viasfora Theme Files (*.vsftheme)|*.vsftheme";

    public MainOptionsControl() {
      InitializeComponent();
    }

    private void ExportButtonClick(object sender, EventArgs e) {
      String filename = GetSaveAsFilename(XML_FILTER);
      if ( String.IsNullOrEmpty(filename) ) {
        return;
      }

      var exporter = SettingsContext.GetService<ISettingsExport>();
      exporter.Export(SettingsContext.GetSettings());
      exporter.Export(SettingsContext.GetService<IRainbowSettings>());
      exporter.Export(SettingsContext.GetService<IXmlSettings>());

      var languageFactory = SettingsContext.GetService<ILanguageFactory>();
      foreach ( var lang in languageFactory.GetAllLanguages() ) {
        exporter.Export(lang.Settings);
      }
      exporter.Save(filename);
      MessageBox.Show(this, "Settings exported successfully.", "Viasfora", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void ImportButtonClick(object sender, EventArgs e) {
      String filename = GetOpenFilename(XML_FILTER);
      if ( String.IsNullOrEmpty(filename) ) {
        return;
      }
      var exporter = SettingsContext.GetService<ISettingsExport>();
      exporter.Load(filename);
      var store = SettingsContext.GetService<ITypedSettingsStore>();
      exporter.Import(store);
      store.Save();

      MessageBox.Show(this, "Settings imported successfully.", "Viasfora", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private String GetSaveAsFilename(String filter) {
      using ( var dialog = new SaveFileDialog() ) {
        dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        dialog.Filter = filter;
        var result = dialog.ShowDialog(this);
        if ( result == DialogResult.OK ) {
          return dialog.FileName;
        }
        return null;
      }
    }
    private String GetOpenFilename(String filter) {
      using ( var dialog = new OpenFileDialog() ) {
        dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        dialog.CheckFileExists = true;
        dialog.Filter = filter;
        var result = dialog.ShowDialog(this);
        if ( result == DialogResult.OK ) {
          return dialog.FileName;
        }
        return null;
      }
    }

    private void ExportColors(ISettingsExport exporter) {
      var list = GetClassifications();
      exporter.Export(list);
    }

    private ClassificationList GetClassifications() {
      ClassificationList list = new ClassificationList(new ColorStorage(this.Site));
      list.Load(
        typeof(CodeClassificationDefinitions),
        typeof(RainbowClassifications),
        typeof(XmlClassificationDefinitions)
        );
      return list;
    }

    private void SaveCurrentThemeButtonClick(object sender, EventArgs e) {
      String filename = GetSaveAsFilename(THEME_FILTER);
      if ( String.IsNullOrEmpty(filename) ) {
        return;
      }

      var classifications = GetClassifications();
      classifications.Export(filename);

      MessageBox.Show(this, "Theme saved successfully.", "Viasfora", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void LoadThemeButtonClick(object sender, EventArgs e) {

    }
  }
}