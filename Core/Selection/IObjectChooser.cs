using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoryMaker.Core.Selection
{
    public interface IObjectChooser<T>
    {
        Task<IEnumerable<T>> Choose();
    }
}