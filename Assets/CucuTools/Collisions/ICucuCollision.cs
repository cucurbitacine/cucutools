using System;
using UnityEngine;

namespace CucuTools
{
    /// <summary>
    /// Collision logic
    /// </summary>
    /// <typeparam name="TImpl">Current type implementation</typeparam>
    /// <typeparam name="TArg">Argument of actions</typeparam>
    public interface ICucuCollision<out TImpl, out TArg> where TImpl : ICucuCollision<TImpl, TArg>
    {
        /// <summary>
        /// Active state
        /// </summary>
        bool IsEnabled { get; set; }
        
        /// <summary>
        /// Layer mask of collision
        /// </summary>
        LayerMask LayerMask { get; set; }
        
        /// <summary>
        /// Set active
        /// </summary>
        /// <param name="value">New active state</param>
        /// <returns>Current implementation</returns>
        TImpl SetEnable(bool value);
        
        /// <summary>
        /// Set layer mask
        /// </summary>
        /// <param name="newLayerMask">New layer mask</param>
        /// <returns>Current implementation</returns>
        TImpl SetLayerMask(LayerMask newLayerMask);

        /// <summary>
        /// Actions on enter. Each call override previous actions 
        /// </summary>
        /// <param name="actions">Actions</param>
        /// <returns>Current implementation</returns>
        TImpl OnEnter(params Action<TArg>[] actions);
        
        /// <summary>
        /// Actions on stay. Each call override previous actions 
        /// </summary>
        /// <param name="actions">Actions</param>
        /// <returns>Current implementation</returns>
        TImpl OnStay(params Action<TArg>[] actions);
        
        /// <summary>
        /// Actions on exit. Each call override previous actions 
        /// </summary>
        /// <param name="actions">Actions</param>
        /// <returns>Current implementation</returns>
        TImpl OnExit(params Action<TArg>[] actions);

        /// <summary>
        /// Return valid state
        /// </summary>
        /// <returns>Valid state</returns>
        bool IsValid();

        /// <summary>
        /// Try to become valid
        /// </summary>
        /// <returns>Valid state</returns>
        bool Validation();
    }
}