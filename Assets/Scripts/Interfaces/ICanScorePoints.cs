using System;
using UnityEngine;

public interface ICanScorePoints {

    // gameObject is needed for finding the entity's transform position
    // This is inherited from MonoBehaviour and doesn't need explicit implementation
    GameObject gameObject { get; }

    // The PointValue property is set privately by each entity internally,
    // based on a public field in the Inspector (declared in the entity).
    // We only need a getter here.
    int PointValue { get; }

    // This boolean property is set by each entity internally,
    // based on a public field in the Inspector (declared in the entity).
    // This allows selecting, in the prefab, whether the entity
    // should show its score value at its position when destroyed.
    bool DisplayPointsWhenKilled { get; }

    // Action<ICanScorePoints> OnScorePoints { get; set; }
    // ScoreManager ScoreManager { set; }
}