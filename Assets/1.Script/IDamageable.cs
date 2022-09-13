public interface IDamageable
{
    //공격이 성공적으로 되면 true 가된다.
    bool ApplyDamage(DamageMessage damageMessage);
}