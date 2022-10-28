using UnityEngine;

public interface IItem
{
    //PlayerController 스크립트를 통하여 아이템이 사용되는 동작이 실행된다.
    void Use(GameObject target);
}