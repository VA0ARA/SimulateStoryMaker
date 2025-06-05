using System;
using System.Collections.Generic;

namespace StoryMaker.Models.Interfaces
{
    public interface IComponentController
    {
        T AddComponent<T>() where T : Components.Component , new();
        T AddComponent<T>(T t) where T : Components.Component;
        void RemoveComponent(Components.Component component);

        // return T component
        T GetComponent<T>() where T : Components.Component;

        // return list of T component
        IEnumerable<T> GetComponents<T>() where T : Components.Component;

        Components.Component GetComponentByType(Type type);

        // defined as parameter for M-V-VM structure
        IEnumerable<Components.Component> Components { get; }
    }
}