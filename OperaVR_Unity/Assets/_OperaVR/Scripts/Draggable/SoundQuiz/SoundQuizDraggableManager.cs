using static OperaVR.DragAndDropQuizPopupData;

namespace OperaVR
{
    public class SoundQuizDraggableManager : DraggableManager
    {
        public override void LoadData(DraggableSlotData[] draggableSlotsData, DraggableData[] draggablesData)
        {
            base.LoadData(draggableSlotsData, draggablesData);

            for (var i = 0; i < Draggables.Count; i++)
            {
                var draggable = Draggables[i] as SoundQuizDraggable;
                if (i < draggablesData.Length)
                {
                    var draggableData = draggablesData[i];
                    draggable.Image.sprite = draggableData.Sprite;
                    continue;
                }
            }
        }
    }
}
