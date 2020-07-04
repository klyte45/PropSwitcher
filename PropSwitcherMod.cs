using ColossalFramework.Globalization;
using ColossalFramework.UI;
using Klyte.Commons.Extensors;
using Klyte.Commons.Interfaces;
using Klyte.Commons.Utils;
using System.IO;
using System.Reflection;
using UnityEngine;

[assembly: AssemblyVersion("0.0.0.*")]
namespace Klyte.PropSwitcher
{
    public class PropSwitcherMod : BasicIUserMod<PropSwitcherMod, PSController, PSPanel>
    {

        public override string IconName => "K45_PS_Icon";
        public override string SimpleName => "Prop Switcher";
        public override string Description => "Simple switch from a prop model to another in all their uses";

        public override void TopSettingsUI(UIHelperExtension ext) => AddFolderButton(PSController.DefaultGlobalPropConfigurationFolder, ext, "K45_PS_GLOBALREPLACEMENTPROPFOLDER");

        private static void AddFolderButton(string filePath, UIHelperExtension helper, string localeId)
        {
            FileInfo fileInfo = FileUtils.EnsureFolderCreation(filePath);
            helper.AddLabel(Locale.Get(localeId) + ":");
            var namesFilesButton = ((UIButton)helper.AddButton("/", () => ColossalFramework.Utils.OpenInFileBrowser(fileInfo.FullName)));
            namesFilesButton.textColor = Color.yellow;
            KlyteMonoUtils.LimitWidth(namesFilesButton, 710);
            namesFilesButton.text = fileInfo.FullName + Path.DirectorySeparatorChar;
        }
    }
}