namespace OperaVR
{
    public class QuickPopupData : APopupData
    {
        public float LogTime = 4;

        public static QuickPopupData GenerateData(
            string text, string textLocalizationKey = "", float time = 4f)
        {
            var newInstance = CreateInstance<QuickPopupData>();

            newInstance.PopupType = PopupType.Quick;
            newInstance.Title = "";
            newInstance.TitleKey = "";
            newInstance.SubTitle = text;
            newInstance.SubTitleKey = textLocalizationKey;
            newInstance.LogTime = time;

            return newInstance;
        }
    }
}