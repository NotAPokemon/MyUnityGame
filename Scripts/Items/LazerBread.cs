using UnityEngine;

public class LazerBread : BaseItem
{
    float lineTime = 0.5f;
    LineRenderer lineRenderer;

    protected override void Start()
    {
        base.Start();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

    }

    protected override void rightClick()
    {
        if (lineTime >= 0.5f && Player.player.mana >= 10)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            lineRenderer.enabled = true;
            lineTime = 0;

            Player.player.mana -= 10;

            lineRenderer.SetPosition(0, ray.origin + new Vector3(0, 0, 0.1f));
            if (Physics.SphereCast(ray, 3, out hit))
            {
                if (Vector3.Distance(hit.transform.position, transform.position) <= 300)
                {
                    BaseEntity hitEntity = hit.transform.GetComponent<BaseEntity>();
                    if (hitEntity != null)
                    {
                        hitEntity.health -= (damage + FindObjectOfType<Player>().damageAmount);
                    }
                }
            }

            lineRenderer.SetPosition(1, ray.origin + ray.direction * 300);
        }
    }

    protected override void Update()
    {
        base.Update();
        lineTime += Time.deltaTime;
        if (lineTime >= 0.5f)
        {
            lineRenderer.enabled = false;
        }
    }
}
