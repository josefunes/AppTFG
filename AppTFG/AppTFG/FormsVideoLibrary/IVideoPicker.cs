using System;
using System.Threading.Tasks;

namespace AppTFG.FormsVideoLibrary
{
    public interface IVideoPicker
    {
        Task<string> GetVideoFileAsync();
    }
}

