//An interface for different tower type scripts
public interface ITower
{
    public void OnBuild(SelectableTile _tile);
    public void Upgrade(float _range, float _attackInterval);
    public void Destroy();
    public float GetCurrentRange();
    public float GetCurrentAttackInterval();
    public bool IsFullyUpgraded();
    public int GetTheCurrentLevel();
    public WeaponScriptableObject GetWeaponScriptableObject();
}