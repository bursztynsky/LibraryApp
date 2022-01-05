using LibraryApp.DataAccess.Data;
using LibraryApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // To metoda ktora zostanie wywolana jesli uzyjemy wskaznika asp-action = "Index"
        // Metoda ta korzystac z ApplicationDbContext by pobrac wszystkie kategorie oraz przekazuje
        // je do View, ktotre nastepnie odpowiada za jego wyswietlenie
        // Korzystamy tu ze slowa return, poniewaz zwracamy uzytkownikowi finalnie storne z wygenerowana zawartoscia
        // Jest to metoda HTTP GET
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _dbContext.Categories;
            return View(objCategoryList);
        }

        // Teraz dodajemy metode odpowiedzialna za klikniecie przycisku 'create'
        // Metoda ma tylko jedno zadanie - zwraca widok, ktory Nam umozliwia dodanie nowej kategorii
        public IActionResult Create()
        {
            return View();
        }

        // Kiedy na stronie Create.cshtml zostanie klikniety button typu submit to zostanie wyslana komenda HTTP POST
        // z prosba o tworzenie kategorii o podanych danych. Z tego powodu musimy dodac metode ktora przyjmie ta komende
        // i, jesli dane sa prawidlowe (wszystkie odpowiednie pola wypelnione) doda kategorie do bazy danych
        [HttpPost]
        [ValidateAntiForgeryToken] // ten atrybut upewnia sie ze nie zostanie to wykorzystane poza nasza aplikacja
        // W tym wypadku mozemy stworzyc metode o tej samej nazwie co powyzej, poniewaz ma inny argument - jest on typu Category
        // Dzieki temu nasza strona Create.cshtml wie, ze dziala na modelu Category wiec jest w stanie wyslac nam taki sam obiekt
        // do naszego kontrolera
        public IActionResult Create(Category obj)
        {
            // Najpierw korzystajac z gotowej funkcjonalnosci sprawdzamy czy wszystkie dane w wyslanej kategorii za wypelnione
            // Jesli nie, to zwracamy widok przez co uzytkownik znow zobaczy strone do tworzenia kategorii
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Tu juz wiemy, ze model zostal poprawnie stworzony, wiec dodajemy go do bazy za pomoca kontekstu 
            // oraz zapisujemy zmiany
            _dbContext.Categories.Add(obj);
            _dbContext.SaveChanges();

            // Na sam koniec nie zwracamy strony Create.cshtml tylko przekierowujemy uzytkownika do strony glownej kategorii, czyli Index
            return RedirectToAction(nameof(Index));
        }

        // Teraz dodamy czesc odpowiedzialna za edyjce
        public IActionResult Edit(int? id)
        {
            // Poniewaz id jest opcjonalne, ktoz moze wyslac zapytanie bez niego, to sprawdzamy, czy ono istnieje
            // jesli nie, to zwracamy strone NotFound
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Teraz szukamy danego obiektu, ktory zostal edytowany, w Naszej bazie
            // Find szuka po primary key w tym przypadku jest to int Id
            var categoryFromDb = _dbContext.Categories.Find(id);
            
            // Teraz sprawdzamy, czy baza zwrocila jakis obiekt
            // Jesli jest null, to NotFound
            if(categoryFromDb == null)
            {
                return NotFound();
            }

            // Jesli wszystko sie zgadza, to obiekt z bazy przekazujemy do wydoku by ten wyslal wszystkie dane uzytkownikowi
            return View(categoryFromDb);
        }

        // Po kliknieciu na stronie Edit.cshtml button typu submit zostanie ponownie wyslane zapytanie HTTP POST z 
        // nowymi danymi Category, ktore chcemy zapisac w bazie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            // Sprawdzamy czy podano poprawne dane
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Jesli tak, to zapisujemy zmiany
            _dbContext.Categories.Update(obj);
            _dbContext.SaveChanges();

            // Korzystajac z TempData mozemy wyslac klientowi komunikat, ktory nastepnie mozemy w jaki sposob wyswietlic
            TempData["success"] = "Category updated successfully";

            // Oraz przesuwamy uzytkownika do Index page
            return RedirectToAction(nameof(Index));
        }

        // Teraz dodajemy funkcjonalnosc odpowiedzialna za usuwanie kategorii z bazy danych
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = _dbContext.Categories.Find(id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        // Teraz metoda HTTP POST ktora zajmie sie usuwaniem
        // Mozemy zamiast argumentu Category podac po prostu id, ale to oznacza dwie zmiany:
        // 1. musimy podac inna nazwe metody (i podac ja w widoku) LUB dodac atrybut ActionName("Delete") -> wtedy nie musimy zmieniac nazwy w formularzu
        // 2. umiescic w widoku ukryte pole z id, poniewaz inaczej, nasz widok go nie wysle! To dziala z automatu tylko jak uzyjemy modelu a nie id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _dbContext.Categories.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            _dbContext.Categories.Remove(obj);
            _dbContext.SaveChanges();

            // Temp data sa tylko az do wyslania jednej odpowiedzi od serwera - potem jak uzytkownik znow wysle HTTP GET lub POST to zostana one usuniete
            // z przegladarki
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
