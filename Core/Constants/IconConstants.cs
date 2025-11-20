using System.Windows.Media;

namespace TechDashboard.Core.Constants;

/// <summary>
/// Application icon constants using Segoe MDL2 Assets font
/// Provides centralized icon definitions for consistent usage across the application
/// Based on Microsoft Segoe MDL2 Assets: https://learn.microsoft.com/windows/apps/design/style/segoe-ui-symbol-font
/// </summary>
public static class IconConstants
{
    /// <summary>
    /// Default font family name for all icons
    /// </summary>
    public const string DefaultFontFamilyName = "Segoe MDL2 Assets";

    /// <summary>
    /// Default font family object for all icons (use this in XAML bindings)
    /// </summary>
    public static readonly FontFamily DefaultFontFamily = new FontFamily(DefaultFontFamilyName);

    /// <summary>
    /// Common UI icons used throughout the application
    /// </summary>
    public static class Common
    {
        /// <summary>Add/New icon (&#xE710;)</summary>
        public const string Add = "\uE710";

        /// <summary>Cancel/Close icon (&#xE711;)</summary>
        public const string Cancel = "\uE711";

        /// <summary>More options icon (&#xE712;)</summary>
        public const string More = "\uE712";

        /// <summary>Settings icon (&#xE713;)</summary>
        public const string Settings = "\uE713";

        /// <summary>Search icon (&#xE721;)</summary>
        public const string Search = "\uE721";

        /// <summary>Refresh icon (&#xE72C;)</summary>
        public const string Refresh = "\uE72C";

        /// <summary>Share icon (&#xE72D;)</summary>
        public const string Share = "\uE72D";

        /// <summary>Lock icon (&#xE72E;)</summary>
        public const string Lock = "\uE72E";

        /// <summary>Unlock icon (&#xE785;)</summary>
        public const string Unlock = "\uE785";

        /// <summary>Checkmark icon (&#xE73E;)</summary>
        public const string CheckMark = "\uE73E";

        /// <summary>Fullscreen icon (&#xE740;)</summary>
        public const string FullScreen = "\uE740";

        /// <summary>Print icon (&#xE749;)</summary>
        public const string Print = "\uE749";

        /// <summary>Delete icon (&#xE74D;)</summary>
        public const string Delete = "\uE74D";

        /// <summary>Save icon (&#xE74E;)</summary>
        public const string Save = "\uE74E";

        /// <summary>Cloud icon (&#xE753;)</summary>
        public const string Cloud = "\uE753";

        /// <summary>Light/Theme toggle icon (&#xE793;)</summary>
        public const string Light = "\uE793";

        /// <summary>Undo icon (&#xE7A7;)</summary>
        public const string Undo = "\uE7A7";

        /// <summary>Redo icon (&#xE7A6;)</summary>
        public const string Redo = "\uE7A6";

        /// <summary>Home/Overview icon (&#xE80F;)</summary>
        public const string Home = "\uE80F";

        /// <summary>Calendar icon (&#xE787;)</summary>
        public const string Calendar = "\uE787";

        /// <summary>Copy icon (&#xE8C8;)</summary>
        public const string Copy = "\uE8C8";

        /// <summary>Paste icon (&#xE77F;)</summary>
        public const string Paste = "\uE77F";

        /// <summary>Cut icon (&#xE8C6;)</summary>
        public const string Cut = "\uE8C6";

        /// <summary>Document/Reports icon (&#xE8A5;)</summary>
        public const string Document = "\uE8A5";

        /// <summary>Open file icon (&#xE8E5;)</summary>
        public const string OpenFile = "\uE8E5";

        /// <summary>Folder icon (&#xE8B7;)</summary>
        public const string Folder = "\uE8B7";

        /// <summary>Folder open icon (&#xE838;)</summary>
        public const string FolderOpen = "\uE838";

        /// <summary>New folder icon (&#xE8F4;)</summary>
        public const string NewFolder = "\uE8F4";

        /// <summary>Chrome minimize icon (&#xE921;)</summary>
        public const string ChromeMinimize = "\uE921";

        /// <summary>Chrome maximize icon (&#xE922;)</summary>
        public const string ChromeMaximize = "\uE922";

        /// <summary>Chrome restore icon (&#xE923;)</summary>
        public const string ChromeRestore = "\uE923";

        /// <summary>Chrome close icon (&#xE8BB;)</summary>
        public const string ChromeClose = "\uE8BB";

        /// <summary>Info icon (&#xE946;)</summary>
        public const string Info = "\uE946";

        /// <summary>Help icon (&#xE897;)</summary>
        public const string Help = "\uE897";

        /// <summary>Analytics/Chart icon (&#xE9D9;)</summary>
        public const string Analytics = "\uE9D9";

        /// <summary>Language icon (&#xF2B7;)</summary>
        public const string Language = "\uF2B7";

        /// <summary>Download icon (&#xE896;)</summary>
        public const string Download = "\uE896";

        /// <summary>Upload icon (&#xE898;)</summary>
        public const string Upload = "\uE898";

        /// <summary>Sync icon (&#xE895;)</summary>
        public const string Sync = "\uE895";

        /// <summary>View icon (&#xE890;)</summary>
        public const string View = "\uE890";

        /// <summary>Edit icon (&#xE70F;)</summary>
        public const string Edit = "\uE70F";

        /// <summary>Clear icon (&#xE894;)</summary>
        public const string Clear = "\uE894";

        /// <summary>Filter icon (&#xE71C;)</summary>
        public const string Filter = "\uE71C";

        /// <summary>Sort icon (&#xE8CB;)</summary>
        public const string Sort = "\uE8CB";

        /// <summary>Zoom in icon (&#xE8A3;)</summary>
        public const string ZoomIn = "\uE8A3";

        /// <summary>Zoom out icon (&#xE71F;)</summary>
        public const string ZoomOut = "\uE71F";
    }

    /// <summary>
    /// Navigation icons
    /// </summary>
    public static class Navigation
    {
        /// <summary>Chevron down icon (&#xE70D;)</summary>
        public const string ChevronDown = "\uE70D";

        /// <summary>Chevron up icon (&#xE70E;)</summary>
        public const string ChevronUp = "\uE70E";

        /// <summary>Chevron left icon (&#xE76B;)</summary>
        public const string ChevronLeft = "\uE76B";

        /// <summary>Chevron right icon (&#xE76C;)</summary>
        public const string ChevronRight = "\uE76C";

        /// <summary>Back icon (&#xE72B;)</summary>
        public const string Back = "\uE72B";

        /// <summary>Forward icon (&#xE72A;)</summary>
        public const string Forward = "\uE72A";

        /// <summary>Up icon (&#xE74A;)</summary>
        public const string Up = "\uE74A";

        /// <summary>Down icon (&#xE74B;)</summary>
        public const string Down = "\uE74B";

        /// <summary>Previous icon (&#xE892;)</summary>
        public const string Previous = "\uE892";

        /// <summary>Next icon (&#xE893;)</summary>
        public const string Next = "\uE893";

        /// <summary>Page left icon (&#xE760;)</summary>
        public const string PageLeft = "\uE760";

        /// <summary>Page right icon (&#xE761;)</summary>
        public const string PageRight = "\uE761";

        /// <summary>All apps icon (&#xE71D;)</summary>
        public const string AllApps = "\uE71D";

        /// <summary>Go to start icon (&#xE8FC;)</summary>
        public const string GoToStart = "\uE8FC";
    }

    /// <summary>
    /// Status and feedback icons
    /// </summary>
    public static class Status
    {
        /// <summary>Success/Checkmark icon (&#xE73E;)</summary>
        public const string Success = "\uE73E";

        /// <summary>Error icon (&#xE783;)</summary>
        public const string Error = "\uE783";

        /// <summary>Warning icon (&#xE7BA;)</summary>
        public const string Warning = "\uE7BA";

        /// <summary>Info icon (&#xE946;)</summary>
        public const string Info = "\uE946";

        /// <summary>Completed icon (&#xE930;)</summary>
        public const string Completed = "\uE930";

        /// <summary>Important icon (&#xE8C9;)</summary>
        public const string Important = "\uE8C9";

        /// <summary>Sync error icon (&#xEA6A;)</summary>
        public const string SyncError = "\uEA6A";

        /// <summary>Status circle icon (&#xEA81;)</summary>
        public const string StatusCircle = "\uEA81";

        /// <summary>Status error icon (&#xEA83;)</summary>
        public const string StatusError = "\uEA83";

        /// <summary>Status warning icon (&#xEA84;)</summary>
        public const string StatusWarning = "\uEA84";
    }

    /// <summary>
    /// Media and playback icons
    /// </summary>
    public static class Media
    {
        /// <summary>Play icon (&#xE768;)</summary>
        public const string Play = "\uE768";

        /// <summary>Pause icon (&#xE769;)</summary>
        public const string Pause = "\uE769";

        /// <summary>Stop icon (&#xE71A;)</summary>
        public const string Stop = "\uE71A";

        /// <summary>Record icon (&#xE7C8;)</summary>
        public const string Record = "\uE7C8";

        /// <summary>Volume icon (&#xE767;)</summary>
        public const string Volume = "\uE767";

        /// <summary>Mute icon (&#xE74F;)</summary>
        public const string Mute = "\uE74F";

        /// <summary>Fast forward icon (&#xEB9D;)</summary>
        public const string FastForward = "\uEB9D";

        /// <summary>Rewind icon (&#xEB9E;)</summary>
        public const string Rewind = "\uEB9E";

        /// <summary>Repeat all icon (&#xE8EE;)</summary>
        public const string RepeatAll = "\uE8EE";

        /// <summary>Repeat one icon (&#xE8ED;)</summary>
        public const string RepeatOne = "\uE8ED";

        /// <summary>Shuffle icon (&#xE8B1;)</summary>
        public const string Shuffle = "\uE8B1";

        /// <summary>Video icon (&#xE714;)</summary>
        public const string Video = "\uE714";

        /// <summary>Camera icon (&#xE722;)</summary>
        public const string Camera = "\uE722";

        /// <summary>Music note icon (&#xEC4F;)</summary>
        public const string MusicNote = "\uEC4F";

        /// <summary>Picture icon (&#xE8B9;)</summary>
        public const string Picture = "\uE8B9";

        /// <summary>Webcam icon (&#xE8B8;)</summary>
        public const string Webcam = "\uE8B8";
    }

    /// <summary>
    /// Communication icons
    /// </summary>
    public static class Communication
    {
        /// <summary>Mail icon (&#xE715;)</summary>
        public const string Mail = "\uE715";

        /// <summary>Send icon (&#xE724;)</summary>
        public const string Send = "\uE724";

        /// <summary>Message icon (&#xE8BD;)</summary>
        public const string Message = "\uE8BD";

        /// <summary>Phone icon (&#xE717;)</summary>
        public const string Phone = "\uE717";

        /// <summary>Video chat icon (&#xE8AA;)</summary>
        public const string VideoChat = "\uE8AA";

        /// <summary>Contact icon (&#xE77B;)</summary>
        public const string Contact = "\uE77B";

        /// <summary>People icon (&#xE716;)</summary>
        public const string People = "\uE716";

        /// <summary>Mail reply icon (&#xE8CA;)</summary>
        public const string MailReply = "\uE8CA";

        /// <summary>Mail reply all icon (&#xE8C2;)</summary>
        public const string MailReplyAll = "\uE8C2";

        /// <summary>Mail forward icon (&#xE89C;)</summary>
        public const string MailForward = "\uE89C";

        /// <summary>Emoji icon (&#xE899;)</summary>
        public const string Emoji = "\uE899";

        /// <summary>Attach icon (&#xE723;)</summary>
        public const string Attach = "\uE723";
    }

    /// <summary>
    /// Text editing and formatting icons
    /// </summary>
    public static class TextFormatting
    {
        /// <summary>Bold icon (&#xE8DD;)</summary>
        public const string Bold = "\uE8DD";

        /// <summary>Italic icon (&#xE8DB;)</summary>
        public const string Italic = "\uE8DB";

        /// <summary>Underline icon (&#xE8DC;)</summary>
        public const string Underline = "\uE8DC";

        /// <summary>Font icon (&#xE8D2;)</summary>
        public const string Font = "\uE8D2";

        /// <summary>Font color icon (&#xE8D3;)</summary>
        public const string FontColor = "\uE8D3";

        /// <summary>Font size icon (&#xE8E9;)</summary>
        public const string FontSize = "\uE8E9";

        /// <summary>Font increase icon (&#xE8E8;)</summary>
        public const string FontIncrease = "\uE8E8";

        /// <summary>Font decrease icon (&#xE8E7;)</summary>
        public const string FontDecrease = "\uE8E7";

        /// <summary>Align left icon (&#xE8E4;)</summary>
        public const string AlignLeft = "\uE8E4";

        /// <summary>Align center icon (&#xE8E3;)</summary>
        public const string AlignCenter = "\uE8E3";

        /// <summary>Align right icon (&#xE8E2;)</summary>
        public const string AlignRight = "\uE8E2";

        /// <summary>Bulleted list icon (&#xE8FD;)</summary>
        public const string BulletedList = "\uE8FD";

        /// <summary>Highlight icon (&#xE7E6;)</summary>
        public const string Highlight = "\uE7E6";
    }

    /// <summary>
    /// Device and hardware icons
    /// </summary>
    public static class Devices
    {
        /// <summary>Devices icon (&#xE772;)</summary>
        public const string DevicesIcon = "\uE772";

        /// <summary>Tablet icon (&#xE70A;)</summary>
        public const string Tablet = "\uE70A";

        /// <summary>Mobile tablet icon (&#xE8CC;)</summary>
        public const string MobileTablet = "\uE8CC";

        /// <summary>Cell phone icon (&#xE8EA;)</summary>
        public const string CellPhone = "\uE8EA";

        /// <summary>Keyboard icon (&#xE92E;)</summary>
        public const string Keyboard = "\uE92E";

        /// <summary>Mouse icon (&#xE962;)</summary>
        public const string Mouse = "\uE962";

        /// <summary>Headphone icon (&#xE7F6;)</summary>
        public const string Headphone = "\uE7F6";

        /// <summary>Speakers icon (&#xE7F5;)</summary>
        public const string Speakers = "\uE7F5";

        /// <summary>TV monitor icon (&#xE7F4;)</summary>
        public const string TVMonitor = "\uE7F4";

        /// <summary>Projector icon (&#xE95D;)</summary>
        public const string Projector = "\uE95D";

        /// <summary>Game console icon (&#xE967;)</summary>
        public const string GameConsole = "\uE967";
    }

    /// <summary>
    /// Network and connectivity icons
    /// </summary>
    public static class Network
    {
        /// <summary>WiFi icon (&#xE701;)</summary>
        public const string Wifi = "\uE701";

        /// <summary>Bluetooth icon (&#xE702;)</summary>
        public const string Bluetooth = "\uE702";

        /// <summary>Airplane mode icon (&#xE709;)</summary>
        public const string Airplane = "\uE709";

        /// <summary>Ethernet icon (&#xE839;)</summary>
        public const string Ethernet = "\uE839";

        /// <summary>VPN icon (&#xE705;)</summary>
        public const string VPN = "\uE705";

        /// <summary>Network icon (&#xE968;)</summary>
        public const string NetworkIcon = "\uE968";

        /// <summary>Signal bars icon (&#xE870;)</summary>
        public const string SignalBars = "\uE870";

        /// <summary>Globe icon (&#xE774;)</summary>
        public const string Globe = "\uE774";

        /// <summary>World icon (&#xE909;)</summary>
        public const string World = "\uE909";
    }

    /// <summary>
    /// System and settings icons
    /// </summary>
    public static class System
    {
        /// <summary>System icon (&#xE770;)</summary>
        public const string SystemIcon = "\uE770";

        /// <summary>Admin icon (&#xE7EF;)</summary>
        public const string Admin = "\uE7EF";

        /// <summary>Permissions icon (&#xE8D7;)</summary>
        public const string Permissions = "\uE8D7";

        /// <summary>Update restore icon (&#xE777;)</summary>
        public const string UpdateRestore = "\uE777";

        /// <summary>Power button icon (&#xE7E8;)</summary>
        public const string PowerButton = "\uE7E8";

        /// <summary>Battery icon (&#xE83F;)</summary>
        public const string Battery = "\uE83F";

        /// <summary>Brightness icon (&#xE706;)</summary>
        public const string Brightness = "\uE706";

        /// <summary>Volume bars icon (&#xEBC5;)</summary>
        public const string VolumeBars = "\uEBC5";

        /// <summary>Personalize icon (&#xE771;)</summary>
        public const string Personalize = "\uE771";

        /// <summary>Time language icon (&#xE775;)</summary>
        public const string TimeLanguage = "\uE775";

        /// <summary>Ease of access icon (&#xE776;)</summary>
        public const string EaseOfAccess = "\uE776";
    }

    /// <summary>
    /// File and document icons
    /// </summary>
    public static class Files
    {
        /// <summary>Page icon (&#xE7C3;)</summary>
        public const string Page = "\uE7C3";

        /// <summary>Protected document icon (&#xE8A6;)</summary>
        public const string ProtectedDocument = "\uE8A6";

        /// <summary>PDF icon (&#xEA90;)</summary>
        public const string PDF = "\uEA90";

        /// <summary>Library icon (&#xE8F1;)</summary>
        public const string Library = "\uE8F1";

        /// <summary>Save as icon (&#xE792;)</summary>
        public const string SaveAs = "\uE792";

        /// <summary>Save local icon (&#xE78C;)</summary>
        public const string SaveLocal = "\uE78C";

        /// <summary>Save copy icon (&#xEA35;)</summary>
        public const string SaveCopy = "\uEA35";

        /// <summary>Import icon (&#xE8B5;)</summary>
        public const string Import = "\uE8B5";

        /// <summary>Export icon (&#xEDE1;)</summary>
        public const string Export = "\uEDE1";

        /// <summary>Bookmarks icon (&#xE8A4;)</summary>
        public const string Bookmarks = "\uE8A4";

        /// <summary>Recent icon (&#xE823;)</summary>
        public const string Recent = "\uE823";

        /// <summary>History icon (&#xE81C;)</summary>
        public const string History = "\uE81C";
    }

    /// <summary>
    /// Location and map icons
    /// </summary>
    public static class Location
    {
        /// <summary>Map pin icon (&#xE707;)</summary>
        public const string MapPin = "\uE707";

        /// <summary>Location icon (&#xE81D;)</summary>
        public const string LocationIcon = "\uE81D";

        /// <summary>Map directions icon (&#xE816;)</summary>
        public const string MapDirections = "\uE816";

        /// <summary>World icon (&#xE909;)</summary>
        public const string World = "\uE909";

        /// <summary>Street icon (&#xE913;)</summary>
        public const string Street = "\uE913";

        /// <summary>Car icon (&#xE804;)</summary>
        public const string Car = "\uE804";

        /// <summary>Bus icon (&#xE806;)</summary>
        public const string Bus = "\uE806";

        /// <summary>Train icon (&#xE7C0;)</summary>
        public const string Train = "\uE7C0";

        /// <summary>Airplane solid icon (&#xEB4C;)</summary>
        public const string AirplaneSolid = "\uEB4C";

        /// <summary>Work icon (&#xE821;)</summary>
        public const string Work = "\uE821";
    }

    /// <summary>
    /// Shopping and commerce icons
    /// </summary>
    public static class Commerce
    {
        /// <summary>Shop icon (&#xE719;)</summary>
        public const string Shop = "\uE719";

        /// <summary>Shopping cart icon (&#xE7BF;)</summary>
        public const string ShoppingCart = "\uE7BF";

        /// <summary>Payment card icon (&#xE8C7;)</summary>
        public const string PaymentCard = "\uE8C7";

        /// <summary>Market icon (&#xEAFC;)</summary>
        public const string Market = "\uEAFC";

        /// <summary>Cafe icon (&#xEC32;)</summary>
        public const string Cafe = "\uEC32";

        /// <summary>Package icon (&#xE7B8;)</summary>
        public const string Package = "\uE7B8";
    }

    /// <summary>
    /// Social and rating icons
    /// </summary>
    public static class Social
    {
        /// <summary>Like icon (&#xE8E1;)</summary>
        public const string Like = "\uE8E1";

        /// <summary>Dislike icon (&#xE8E0;)</summary>
        public const string Dislike = "\uE8E0";

        /// <summary>Heart icon (&#xEB51;)</summary>
        public const string Heart = "\uEB51";

        /// <summary>Heart fill icon (&#xEB52;)</summary>
        public const string HeartFill = "\uEB52";

        /// <summary>Favorite star icon (&#xE734;)</summary>
        public const string FavoriteStar = "\uE734";

        /// <summary>Favorite star fill icon (&#xE735;)</summary>
        public const string FavoriteStarFill = "\uE735";

        /// <summary>Comment icon (&#xE90A;)</summary>
        public const string Comment = "\uE90A";

        /// <summary>Share icon (&#xE72D;)</summary>
        public const string Share = "\uE72D";
    }

    /// <summary>
    /// Tools and utilities icons
    /// </summary>
    public static class Tools
    {
        /// <summary>Calculator icon (&#xE8EF;)</summary>
        public const string Calculator = "\uE8EF";

        /// <summary>Stopwatch icon (&#xE916;)</summary>
        public const string Stopwatch = "\uE916";

        /// <summary>Color icon (&#xE790;)</summary>
        public const string Color = "\uE790";

        /// <summary>Ruler icon (&#xED5E;)</summary>
        public const string Ruler = "\uED5E";

        /// <summary>Pencil icon (&#xED63;)</summary>
        public const string Pencil = "\uED63";

        /// <summary>Marker icon (&#xED64;)</summary>
        public const string Marker = "\uED64";

        /// <summary>Crop icon (&#xE7A8;)</summary>
        public const string Crop = "\uE7A8";

        /// <summary>Rotate icon (&#xE7AD;)</summary>
        public const string Rotate = "\uE7AD";

        /// <summary>Scan icon (&#xE8FE;)</summary>
        public const string Scan = "\uE8FE";

        /// <summary>QR code icon (&#xED14;)</summary>
        public const string QRCode = "\uED14";

        /// <summary>Bug icon (&#xEBE8;)</summary>
        public const string Bug = "\uEBE8";

        /// <summary>Code icon (&#xE943;)</summary>
        public const string Code = "\uE943";
    }

    /// <summary>
    /// Helper method to get icon with proper format for XAML binding
    /// </summary>
    /// <param name="iconCode">The icon code (e.g., "\uE710")</param>
    /// <returns>Formatted icon string</returns>
    public static string GetIcon(string iconCode) => iconCode;

    /// <summary>
    /// Gets the default font family for icons
    /// </summary>
    public static string GetFontFamily() => DefaultFontFamilyName;
}
