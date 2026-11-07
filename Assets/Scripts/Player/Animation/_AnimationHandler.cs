using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class _AnimationHandler : MonoBehaviour
{
    public enum ClipType
    {
        Movement,
        Sword,
        Spear,
        Bow
    }

    [SerializeField] List<AnimationClip> _movementAnimationClips;
    [SerializeField] List<AnimationClip> _swordAnimationClips;
    [SerializeField] List<AnimationClip> _spearAnimationClips;
    [SerializeField] List<AnimationClip> _bowAnimationClips;

    [SerializeField] public Animation _animator;
    [SerializeField] ClipType _currentClipType;
    string _currentClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponentInChildren<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallClips(ClipType type)
    {
        switch (_currentClipType)
        {
            case ClipType.Sword:
                {
                    ReplaceClips(_swordAnimationClips);
                    break;
                }
            case ClipType.Spear:
                {
                    ReplaceClips(_spearAnimationClips);
                    break;
                }
            case ClipType.Bow:
                {
                    ReplaceClips(_bowAnimationClips);
                    break;
                }
        }
        
        switch (type)
        {
            case ClipType.Sword:
                {
                    AddClips(_swordAnimationClips);
                    _currentClipType = ClipType.Sword;
                    break;
                }
            case ClipType.Spear:
                {
                    AddClips(_spearAnimationClips);
                    _currentClipType = ClipType.Spear;
                    break;
                }
            case ClipType.Bow:
                {
                    AddClips(_bowAnimationClips);
                    _currentClipType = ClipType.Bow;
                    break;
                }
        }
    }

    void ReplaceClips(List<AnimationClip> list)
    {
        foreach (AnimationClip clip in list)
        {
            _animator.RemoveClip(clip);
        }
    }

    void AddClips(List<AnimationClip> list)
    {
        foreach (AnimationClip clip in list)
        {
            _animator.AddClip(clip,clip.name);
        }
    }


    public void LookUp(ClipType type, string name)
    {
        switch (type)
        {
            case ClipType.Sword:
                {
                    CallAnimation(_swordAnimationClips,"_" + type.ToString() + name);
                    break;
                }
            case ClipType.Spear:
                {
                    CallAnimation(_spearAnimationClips, "_" + type.ToString() + name);
                    break;
                }
            case ClipType.Bow:
                {
                    CallAnimation(_bowAnimationClips, "_" + type.ToString() + name);
                    break;
                }
        }
    }

    public void CallAnimation(List<AnimationClip> list, string name)
    {
        foreach (AnimationClip clip in list)
        {
            if (clip.name == name && _currentClip != name)
            {
                _currentClip = clip.name;
                _animator.Play(clip.name);
                
            }
        }
        
    }

    IEnumerator TimeToIdle(float time)
    {
        yield return new WaitForSeconds(time);
        _animator.Play(_currentClipType.ToString() + "PlayerIdle");
    }

}
