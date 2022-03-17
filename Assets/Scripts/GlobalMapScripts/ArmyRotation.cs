using UnityEngine;
using UnityEngine.AI;

public class ArmyRotation : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public float rotationY
    {
        get { return transform.rotation.eulerAngles.y; }
        set
        {
            Vector3 v = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(v.x, value, v.z);
        }
    }
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        TurnOffAutoRotationByNavMeshAgent();
    }
    private void LateUpdate()
    {
        AdjustRotationByVelocity();
    }

    private void AdjustRotationByVelocity()
    {
        if (navMeshAgent.velocity.sqrMagnitude <= Mathf.Epsilon)
            return;
        rotationY = Quaternion.LookRotation(navMeshAgent.velocity.normalized).eulerAngles.y;
    }
    private void TurnOffAutoRotationByNavMeshAgent()
    {
        navMeshAgent.updateRotation = false;
    }

}
