using UnityEngine;

public partial class SpaceInvadersEnemyGroupController
{
    
    
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        // Draw a cube to show the top most rank of enemy
        Gizmos.DrawWireCube(transform.position, new Vector3(columns, 1, 0));
    }
}
