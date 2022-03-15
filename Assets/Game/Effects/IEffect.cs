/* --- Libraries --- */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interface that affects the position.
/// </summary>
public interface IEffect {

    // Applies the effect.
    public bool Check(Vector2 position);

    // Applies the effect.
    public void Apply(ref List<Vector2> positions);

}
