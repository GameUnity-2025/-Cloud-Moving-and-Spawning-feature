using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firing_projectiles : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 15f;             // Tốc độ bay ngang của tên
    public float gravityScale = 1f;                 // Mức độ rơi của tên (0 = không rơi, 2 = rơi nhanh hơn)
    public float maxProjectileDistance = 3f;        // Khoảng cách tối đa mũi tên bay được
    public float fireCooldown = 0.5f;               // Thời gian hồi giữa 2 phát bắn (0.5s)

    private float nextFireTime = 0f;                // Thời điểm sớm nhất có thể bắn tiếp

    private character_movement characterMovementScriptReference;

    public string chief_bird_name = "blue bird sprite_0";
    public GameObject chief_bird;
    private chief_NPC_interaction chief_NPC_interaction_script_reference;

    void Start()
    {
        chief_bird = GameObject.Find(chief_bird_name);
        characterMovementScriptReference = GetComponent<character_movement>();
    }

    void Update()
    {
        chief_NPC_interaction chief_NPC_interaction_script_reference = chief_bird.GetComponent<chief_NPC_interaction>();

        // Bấm phím S để bắn (chỉ khi đã hoàn thành tương tác và đã qua thời gian hồi)
        if (Input.GetKeyDown(KeyCode.S) && chief_NPC_interaction_script_reference.completedinteraction == true && Time.time >= nextFireTime)
        {
            ShootProjectile();
            nextFireTime = Time.time + fireCooldown; // Đặt lại thời điểm bắn tiếp theo
        }
    }

    void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        SpriteRenderer projectileRenderer = projectile.GetComponent<SpriteRenderer>();
        projectileRenderer.sortingOrder = 6;
        projectile.tag = "arrow";

        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb == null)
        {
            projectileRb = projectile.AddComponent<Rigidbody2D>();
        }

        BoxCollider2D projectileBc = projectile.GetComponent<BoxCollider2D>();
        if (projectileBc == null)
        {
            projectileBc = projectile.AddComponent<BoxCollider2D>();
        }

        // Bỏ qua va chạm giữa nhân vật và các mũi tên
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("arrow");
        foreach (GameObject arrow in arrows)
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), arrow.GetComponent<BoxCollider2D>(), true);
        }

        // Thiết lập trọng lực cho tên
        projectileRb.gravityScale = gravityScale;

        // Bắn theo hướng nhân vật
        if (characterMovementScriptReference.isfacingright == true)
        {
            projectileRb.velocity = new Vector2(projectileSpeed, 0);
        }
        else
        {
            projectileRb.velocity = new Vector2(-projectileSpeed, 0);
            projectileRenderer.flipX = true;
        }

        StartCoroutine(DestroyProjectileAfterDistance(projectile));
    }

    IEnumerator DestroyProjectileAfterDistance(GameObject projectile)
    {
        float traveledDistance = 0f;

        while (traveledDistance < maxProjectileDistance)
        {
            traveledDistance += projectileSpeed * Time.deltaTime;
            yield return null;
        }

        Destroy(projectile);
    }
}
