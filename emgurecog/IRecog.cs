using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emgurecog
{
    public interface IRecog
    {
        void Trainer(int id, List<Bitmap> listaImagens);

        bool Execute(Bitmap bitmap);
    }
}
