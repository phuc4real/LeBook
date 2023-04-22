using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeBook.Models.ViewModel
{
    public class CHomeViewModel
    {
        public IEnumerable<Promotion> PromotionCarousel { get; set; }
        public IEnumerable<Promotion> Promotion { get; set; }
        public IEnumerable<Book> BestSeller { get; set; }
        public IEnumerable<Book> NewBook { get; set; }
        public IEnumerable<Book> HotDeal { get; set; }
        public IEnumerable<Book> NewManga { get; set; }
        public IEnumerable<Book> TopManga { get; set; }
        public IEnumerable<Book> NewLightNovel { get; set; }
        public IEnumerable<Book> Cate1 { get; set; }
        public IEnumerable<Book> Cate2 { get; set; }
        public IEnumerable<Book> Cate3 { get; set; }
    }
}
