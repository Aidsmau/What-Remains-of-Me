using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DeathLaser : MonoBehaviour
{
    [SerializeField, Tooltip("Draws the laser in the direction of this target.")]
    private Transform _laserTarget;
    [SerializeField]
    private SpikeTeleport _deathTeleporter; // TODO, why do I have to use the SpikeTeleport.cs? Please refactor.

    [Space(20)]

    [SerializeField, Tooltip("Does the laser just draw the line all the way to the target?")]
    private bool _drawToTarget = true;
    [SerializeField, Tooltip("Do the laser direction and length need to be recalculated every frame? I.e. is the target or the source moving? Be warned that having a lot of updating lasers is bad for performance.")]
    private bool _needsRecalculation = false;
    // TODO: this needs to regenerate the mesh every frame. Granted, it's a super simple mesh, but it's a mesh generation every frame regardless.
    // Rework this so that regeneration is performed ONLY WHEN the properties of the laser should change. E.g. if a laser is obstructed by a door,
    // regenerate the laser once the door is open/closed.

    [Space(15)]

    [SerializeField]
    private string _tag = "Player";
    [SerializeField, Tooltip("How long is the laser? Leave at -1 for auto-distance. If drawToTarget is enabled, this does nothing.")]
    private float _laserDistance = -1;

    [Space(15)]

    [SerializeField]
    private LayerMask _mask;
    [SerializeField, Tooltip("Refresh time for laser hitray after hitting the player.")]
    private float _lockoutTime = 1.5f;

    private const int PlayerHierarchyLayerIndex = 3;

    private LineRenderer _renderer;
    private Vector3[] _points; // invariant: this array has 2 elements.
    private Vector3 _dir;
    private bool _lockout = false;
    private float _realLaserDistance;

    private RaycastHit2D _data;
    private bool _didCastHit; // equivalent to _data.point != Vector2.zero

    private void Awake()
    {
        _renderer = GetComponent<LineRenderer>();
        Recalculate();
    }

    private void Update()
    {
        _didCastHit = DoRaycast(out _data);

        if (_needsRecalculation)
        {
            Recalculate();
        }

        CheckForDeath();
    }

    private void Recalculate()
    {
        _dir = (_laserTarget.position - transform.position).normalized;
        Vector3 targetPos;

        bool needsRealLaserDistance = _laserDistance <= 0;

        _realLaserDistance = needsRealLaserDistance ? Vector3.Distance(transform.position, _laserTarget.position) : _laserDistance;

        // draw laser to target point
        if (_drawToTarget)
        {
            targetPos = _laserTarget.position;
        }
        // draw laser in direction of target point until we hit something
        else if (needsRealLaserDistance && _didCastHit)
        {
            targetPos = _data.point;
            // we don't need to reassign _realLaserDistance to dist(position, data point) here because the death ray itself
            // is already stopped by the collider.
        }
        // draw laser in direction of target for a certain distance.
        else
        {
            targetPos = transform.position + (_dir * _realLaserDistance);
        }

        _points = new Vector3[] { transform.position, targetPos };

        _renderer.SetPositions(_points);
    }

    private bool DoRaycast(out RaycastHit2D data)
    {
        data = Physics2D.Raycast(transform.position, _dir, _realLaserDistance, _mask);

        return data.point != Vector2.zero;
    }

    // TODO: There appears to be a bug if atlas dies while transferring energy: the death effect plays twice. It's obvious to see in the
    // DeathLaser mechanics experiment scene. To fix this, just set the lockout time to something large, like 1.5 seconds.
    private void CheckForDeath()
    {
        // The player has several colliders on their gameobject, somewhere in the hierarchy. Because of this, the death effect can get
        // triggered several times (that's the purpose of lockout). Additionally, only the capsule collider is on a gameobject tagged
        // as the player; the rest are untagged children. Therefore, I need to look through all the parents (limited to 3, since that's
        // the max relevant hierachy level) to find the player tag. This is very sad. Oh well.
        if (!_lockout
            && _didCastHit
            && _data.collider is not null
            && UtilityFunctions.CompareTagOfHierarchy(_data.collider.transform, _tag, out var player, PlayerHierarchyLayerIndex))
        {
            _deathTeleporter.PerformDeath(player.gameObject);
            _lockout = true;

            Invoke(nameof(LockoutCooldownInvocation), _lockoutTime);
        }
    }
    
    // IEnumerators are an optimzation for the future.
    private void LockoutCooldownInvocation() => _lockout = false;
}
