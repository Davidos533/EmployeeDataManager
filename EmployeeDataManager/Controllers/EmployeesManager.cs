using Microsoft.AspNetCore.Mvc;
using EmployeeDataManager.Model;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeDataManager.Controllers
{
    // класс описывающий контрооллер в паттерне MVC
    public class EmployeesManagerController : Controller
    {
        DataBaseManager m_dataBaseHandle;     // ссылка на объект базы данных

        // конструктор
        public EmployeesManagerController(DataBaseManager dataBase)
        {
            m_dataBaseHandle = dataBase;        // сохранение ссылки на объект класса управления БД, для дальнейшего использования, контроллером
        }

        // метод для представления привественной страницы
        [HttpGet]
        public IActionResult WelcomePage()
        {
            return View();      // возвращение представления
        }

        // метод для представления страницы со список работников из БД
        [HttpGet]
        public IActionResult EmployeesList()
        {
            return View(m_dataBaseHandle.GetAllTable());      // возврашение списка работников в представление
        }

        // метод для прелоставлению страницы ввода данных добавляемого сторудника
        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();      // возвращение представлениея
        }

        // метод для обработки POST запроса и записи полученных со страницы данных в БД
        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            // если добавление записи прошло успешно
            if (m_dataBaseHandle.AddWriteToDataBase(employee))
            {
                ViewData["Message"] = "Employee successfuly added";         // отправка сообщения об успешном добавлении
            }
            // иначе если добавление записи прошло безуспешно
            else
            {
                ViewData["Message"] = "Employee not added, some error";     // отправка сообщения об безуспешном добавлении
            }
            return View();      // передача данных представленю
        }

        // метод для обработки GET запроса и удаления записи из БД в соответствии с переданным id 
        [HttpGet]
        public IActionResult DeleteEmployee(int? id)
        {
            // если переданный id не равен null удаление сотрудника по соответствующему id
            if (id != null)
            {
                m_dataBaseHandle.DeleteWriteFromDataBase(id);
            }
            return RedirectToAction("EmployeesList");       // перенаправление в действие вывода списка сотрудников этого контроллера
        }

        // метод для получения страницы с данными работника по id, который был выбран на странице с таблицей всех работников
        [HttpGet]
        public IActionResult EditEmployee(int? id)
        {
            Employee employeeById;      // переменная для хранения и передачи данных сотрудника
            // если по текущему id сотрудник найден то переход на представление редактирования данных сотрудника
            if (m_dataBaseHandle.GetEmployeeById(id, out employeeById))
            {
                return View(employeeById);      // отправка представления с формой для редактирования данных сотрудника
            }
            // иначе если по текущему id сотрудник не найден 
            else
            {
                return RedirectToAction("EmployeesList");        // перенаправление на представление списка сотрудников
            }

        }
        // метод для обновления данных сотрудника, принимает собранную модель сотрудника из формы, и обновляет её в БД
        [HttpPost]
        public IActionResult EditEmployee(Employee editedEmployee)
        {
            // если данные были успешно обновлены
            if (m_dataBaseHandle.EditEmployeeData(editedEmployee))
            {
                ViewData["Message"] = "Employee successfuly edited";        // отправка сообщения об успешном обновлении данных
            }
            // если данные были безуспешно обновлены
            else
            {
                ViewData["Message"] = "Employee not edited, some error";    // отправка сообщения об безуспешном обновлении данных
            }

            return View(editedEmployee);        // возвращение данных сотрудника в представление 
        }

        // метод для получения страницы с формой для удаления нескольких сотрудников 
        [HttpGet]
        public IActionResult DeleteEmployeeRange()
        {
            return View();      // возвращение представления с формой для удаления диапозона сотрудников
        }
        // Post метод для получения строки с диапозоном или список id сотрудников коорых требуется удалить
        [HttpPost]
        public IActionResult DeleteEmployeeRange(string employeesId)
        {
            // если удаление прошло успешно
            if (m_dataBaseHandle.DeleteRangeWritesFeomDataBase(new StringBuilder(employeesId)))     // вызов метода удаления диапозона сотрудников, класса для управления БД
            {
                ViewData["Message"] = "Employees successfuly deleted";                              // отправка сообщения об успешном удалении
            }
            // иначе если удаление прошло безуспешно
            else
            {
                ViewData["Message"] = "Employees not deleted, some error";                          // отправка сообщения об успешном удалении
            }
            return View();      // передача данных представленю
        }
    }
}
    