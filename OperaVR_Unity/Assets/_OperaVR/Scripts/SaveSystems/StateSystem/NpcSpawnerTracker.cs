namespace OperaVR
{
    public class NpcSpawnerTracker : AStateTracker
    {
        protected override string GetVariableKey(string sceneKey) => "NpcSpawnerTracker";
        
        public override void Save(string sceneKey) { }

        public override void Load(string sceneKey) { }
    }
}
