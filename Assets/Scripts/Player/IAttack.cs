using UnityEngine;

// Interface allows us to define a contract, requiring that every attack has this method
// Allows us to avoid a bunch of if statements for each different animal
public interface IAttack
{
    void performAttack(Transform target);

}
