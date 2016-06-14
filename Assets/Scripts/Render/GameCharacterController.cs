using UnityEngine;
using System.Collections;

public class GameCharacterController : GridObjectController {
    public SpriteRenderer typeAura;
    
    public void SetAura(GameCharacter.CHARACTER_TYPE type)
    {
        SpriteRenderer typeSprite = gameObject.transform.Find("TypeAura").GetComponent<SpriteRenderer>();

        switch (type)
        {
            case GameCharacter.CHARACTER_TYPE.BLUE:
                typeSprite.color = new Color(0.15f, 0.15f, 1);
                break;
            case GameCharacter.CHARACTER_TYPE.GREEN:
                typeSprite.color = new Color(0.4f, 1, 0.4f);
                break;
            case GameCharacter.CHARACTER_TYPE.RED:
                typeSprite.color = new Color(1, 0.4f, 0.4f);
                break;
        }
    }

    public void SetHPLength(int heart)
    {
        Vector3 scale = typeAura.transform.localScale;
        typeAura.transform.localScale = new Vector3(heart, scale.y, scale.z);
    }
}
