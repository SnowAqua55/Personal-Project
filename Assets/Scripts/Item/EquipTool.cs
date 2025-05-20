using UnityEngine;

public class EquipTool : Equip
{
	public float attackRate;
	public float attackDistance;
	public float useStamina;
	private bool attacking;

	[Header("Resource Gathering")]
	public bool doesGatherResources;

	[Header("Combat")]
	public bool doesDealDamage;
	public int damage;

	private Animator _animator;
	private Camera _camera;

    void Start()
    {
        _animator = GetComponent<Animator>();
		_camera = Camera.main;
    }

    public override void OnAttackInput()
    {
		if (!attacking)
		{
			if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
			{
				attacking = true;
				_animator.SetTrigger("Attack");
				Invoke("OnCanAttack", attackRate);
			}
		}
    }

	void OnCanAttack()
	{
		attacking = false;
	}

	public void OnHit()
	{
		Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, attackDistance))
		{
			if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
			{
				resource.Gather(hit.point, hit.normal);
				
			}
		}
	}
}