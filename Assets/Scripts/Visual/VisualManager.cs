
using UnityEngine;


public class VisualManager
{
    private CharacterBehaviour host;
    private Animator animator;
    public bool isMoving;

    public VisualManager(CharacterBehaviour character)
    {
        host = character;
        animator = character.animator;
    }

    public enum Orientation
    {
        Down,
        Up,
        Left,
        Right,
    }

    public void ApplyFormVisual(PlayerBehaviour.PlayerForm form)
    {
        bool isHuman = form == PlayerBehaviour.PlayerForm.Human ? true : false;
        animator.SetBool("IsHuman", isHuman);
    }

    public void ApplyOrientationVisual(Vector2? target, Vector2 movementVector)
    {
   
        
        animator.SetInteger("AimOrientation", (int)GetAimOrientation(target));
        animator.SetInteger("MoveOrientation", (int)GetMoveOrientation(movementVector));
        animator.SetBool("IsMoving", isMoving);
    }

    public void ApplyAbilityVisual()
    {
        int ability1 = ApplyAbilityCastingVisual();
        int ability2 = ApplyAbilityExecutingVisual();
        if (ability1 != 0) animator.SetInteger("AbilityID", ability1);
        else if(ability2 != 0) animator.SetInteger("AbilityID", ability2);
        else animator.SetInteger("AbilityID", 0);
    }

    private int ApplyAbilityCastingVisual()
    {
        IAbility ability = host.ObtainCurrentlyCastingAbility();
        if (ability != null)
        {
            animator.SetBool("IsAbilityCasting", true);
            animator.SetInteger("AbilityID", ability.abAnimID);
            return ability.abAnimID;
        }
        else
        {
            animator.SetBool("IsAbilityCasting", false);
            animator.SetInteger("AbilityID", 0);
            return 0;
        }
    }
    
    private int ApplyAbilityExecutingVisual()
    {
        IAbility ability = host.ObtainCurrentlyExecutingAbility();
        if (ability != null)
        {
            animator.SetInteger("AbilityOrientation", (int)ability.abilityOrientation);
            animator.SetBool("IsAbilityExecuting", true);
            animator.SetInteger("AbilityID", ability.abAnimID);
            return ability.abAnimID;
        }
        else
        {
            animator.SetBool("IsAbilityExecuting", false);
            animator.SetInteger("AbilityID", 0);
            return 0;
        }
    }
    

    public Orientation GetAimOrientation(Vector2? target)
    {
        Vector2 direction;
        direction = target != null ? (Vector2)target - (Vector2)host.transform.position : Vector2.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? Orientation.Right : Orientation.Left;
        }
        else
        {
            return direction.y > 0 ? Orientation.Up : Orientation.Down;
        }
    }

    public Orientation GetMoveOrientation(Vector2 movementVector)
    {
        if (Mathf.Abs(movementVector.x) > Mathf.Abs(movementVector.y))
        {
            return movementVector.x > 0 ? Orientation.Right : Orientation.Left;
        }
        else
        {
            return movementVector.y > 0 ? Orientation.Up : Orientation.Down;
        }
    }
}
