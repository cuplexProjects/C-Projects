using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using GeneralToolkitLib.Converters;

namespace ImageView.DataContracts
{
    [Serializable]
    [DataContract(Name = "ImageViewApplicationSettings")]
    public class ImageViewApplicationSettings
    {
        public enum ChangeImageAnimation
        {
            None = 0,
            SlideLeft = 1,
            SlideRight = 2,
            SlideDown = 3,
            SlideUp = 4,
            FadeIn = 5
        }

        public enum WindowDockProximity
        {
            Near = 1,
            Normal = 2,
            Far = 3
        }

        // Instansiate with default settings
        public ImageViewApplicationSettings()
        {
            AlwaysOntop = false;
            AutoRandomizeCollection = true;
            LastUsedSearchPaths = new List<string>();
            ShowImageViewFormsInTaskBar = true;
            NextImageAnimation = ChangeImageAnimation.None;
            ImageTransitionTime = 1000;
            SlideshowInterval = 5000;
            PrimaryImageSizeMode = (int) PictureBoxSizeMode.Zoom;
            PasswordProtectBookmarks = false;
            PasswordDerivedString = "";
            ShowNextPrevControlsOnEnterWindow = true;
            ThumbnailSize = 256;
            MaxThumbnails = 256;
            ConfirmApplicationShutdown = true;
            AutomaticUpdateCheck = true;

            // 128 Mb
            ImageCacheSize = 134217728;
        }

        [DataMember(Name = "ShowImageViewFormsInTaskBar", Order = 1)]
        public bool ShowImageViewFormsInTaskBar { get; set; }

        [DataMember(Name = "AlwaysOntop", Order = 2)]
        public bool AlwaysOntop { get; set; }

        [DataMember(Name = "AutoRandomizeCollection", Order = 3)]
        public bool AutoRandomizeCollection { get; set; }

        [DataMember(Name = "LastUsedSearchPaths", Order = 4)]
        public List<string> LastUsedSearchPaths { get; set; }

        [DataMember(Name = "NextImageAnimation", Order = 5)]
        public ChangeImageAnimation NextImageAnimation { get; set; }

        [DataMember(Name = "SlideshowInterval", Order = 6)]
        public int SlideshowInterval { get; set; }

        [DataMember(Name = "ImageTransitionTime", Order = 7)]
        public int ImageTransitionTime { get; set; }

        [DataMember(Name = "EnableAutoLoadFunctionFromMenu", Order = 8)]
        public bool EnableAutoLoadFunctionFromMenu { get; set; }

        [DataMember(Name = "EnableWindowDocking", Order = 9)]
        public bool EnableWindowDocking { get; set; }

        [DataMember(Name = "WindowDockingProximity", Order = 10)]
        public WindowDockProximity WindowDockingProximity { get; set; }

        [DataMember(Name = "PrimaryImageSizeMode", Order = 11)]
        public int PrimaryImageSizeMode { get; set; }

        [DataMember(Name = "ScreenMinXOffset", Order = 12)]
        public int ScreenMinXOffset { get; set; }

        [DataMember(Name = "ScreenWidthOffset", Order = 13)]
        public int ScreenWidthOffset { get; set; }

        [DataMember(Name = "PasswordProtectBookmarks", Order = 14)]
        public bool PasswordProtectBookmarks { get; protected set; }

        [DataMember(Name = "PasswordDerivedString", Order = 15)]
        public string PasswordDerivedString { get; protected set; }

        [DataMember(Name = "MainFormPosition", Order = 16)]
        public Point MainFormPosition { get; protected set; }

        [DataMember(Name = "MainFormSize", Order = 17)]
        public Size MainFormSize { get; protected set; }

        [DataMember(Name = "UseSavedMainFormPosition", Order = 18)]
        public bool UseSavedMainFormPosition { get; protected set; }

        [DataMember(Name = "ShowSwitchImageButtons", Order = 19)]
        public bool ShowSwitchImageButtons { get; set; }

        [DataMember(Name = "LastFolderLocation", Order = 20)]
        public string LastFolderLocation { get; set; }

        [DataMember(Name = "ShowNextPrevControlsOnEnterWindow", Order = 21)]
        public bool ShowNextPrevControlsOnEnterWindow { get; set; }

        [DataMember(Name = "ImageCacheSize", Order = 22)]
        public long ImageCacheSize { get; set; }

        [DataMember(Name = "DefaultKey", Order = 23)]
        public string DefaultKey { get; set; }

        [DataMember(Name = "ThumbnailSize", Order = 24)]
        public int ThumbnailSize { get; set; }

        [DataMember(Name = "MaxThumbnails", Order = 25)]
        public int MaxThumbnails { get; set; }

        [DataMember(Name = "ThumbnailFormSize", Order = 26)]
        public Size ThumbnailFormSize { get; set; }

        [DataMember(Name = "ThumbnailFormLocation", Order = 27)]
        public Point ThumbnailFormLocation { get; set; }

        [DataMember(Name = "ConfirmApplicationShutdown", Order = 28)]
        public bool ConfirmApplicationShutdown { get; set; }

        [DataMember(Name = "MainWindowBackgroundColor", Order = 29)]
        public bool MainWindowBackgroundColor { get; set; }

        [DataMember(Name = "AutomaticUpdateCheck", Order = 30)]
        public bool AutomaticUpdateCheck { get; set; }

        [DataMember(Name = "LastUpdateCheck", Order = 31)]
        public DateTime? LastUpdateCheck { get; set; }


        public void RemoveDuplicateEntriesWithIgnoreCase()
        {
            var deleteStack = new Stack<string>();
            foreach (string searchPath in LastUsedSearchPaths)
            {
                if (LastUsedSearchPaths.Any(s => s.ToLower() == searchPath))
                    deleteStack.Push(searchPath);
            }

            while (deleteStack.Count > 0)
                LastUsedSearchPaths.Remove(deleteStack.Pop());
        }

        public void DisablePasswordProtectBookmarks()
        {
            PasswordProtectBookmarks = false;
            PasswordDerivedString = null;
        }

        public void EnablePasswordProtectBookmarks(string verifiedPassword)
        {
            if (verifiedPassword != null)
            {
                PasswordProtectBookmarks = true;
                PasswordDerivedString = GeneralConverters.GeneratePasswordDerivedString(verifiedPassword);
                DefaultKey = null;
            }
        }

        public void SetMainFormPosition(Rectangle mainFormBounds)
        {
            Rectangle mainScreenBounds = Screen.PrimaryScreen.Bounds;
            Rectangle intersection = Rectangle.Intersect(mainScreenBounds, mainFormBounds);

            if (intersection != Rectangle.Empty)
            {
                MainFormPosition = new Point(mainFormBounds.X, mainFormBounds.Y);
                MainFormSize = new Size(mainFormBounds.Width, mainFormBounds.Height);
                UseSavedMainFormPosition = true;
            }
        }
    }
}