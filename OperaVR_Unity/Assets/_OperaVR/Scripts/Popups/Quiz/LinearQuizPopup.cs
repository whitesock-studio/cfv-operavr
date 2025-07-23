namespace OperaVR
{
    public class LinearQuizPopup : AQuizPopup
    {
        protected override void OnQuizCompleted()
        {
            base.OnQuizCompleted();

            PopupsManager.Instance.ClosePopup(this);
        }
    }
}