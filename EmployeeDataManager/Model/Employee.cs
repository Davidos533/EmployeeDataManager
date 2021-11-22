using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeDataManager.Model
{
    // класс для описания работника (сотрудника)
    public class Employee
    {
        // поля класса
        private string m_id;
        private string m_name = "N/A";
        private string m_surname = "N/A";
        private string m_patronymic = "N/A";
        private Date m_birthday = new Date(0, 0, 0);
        private string m_group = "N/A";
        private string m_address = "N/A";
        private string m_info = "N/A";

        // свойства для приведения значений, к установленным в БД ограничениям
        public string id
        {
            get;set;
        }
        // имя сотрудника не длинее 32 символов
        public string Name 
        { 
            get 
            { 
                return m_name; 
            } 
            set 
            {
                if (value.Length > 32)
                { 
                    m_name = value.Substring(0, 32);
                }
                m_name = value;
            } 
        }
        // фамилия сотрудника не длинее 32 символов
        public string Surname
        {
            get
            {
                return m_surname;
            }
            set
            {
                if (value.Length > 32)
                {
                    m_surname = value.Substring(0, 32);
                }
                m_surname= value;
            }
        }
        // отчество сотрудника не длинее 32 символов
        public string Patronymic 
        {
            get
            {
                return m_patronymic;
            }
            set
            {
                if (value.Length > 32)
                {
                    m_patronymic = value.Substring(0, 32);
                }
                m_patronymic= value;
            }
        }
        // дата рождения сотрудника не длинее 10 символов
        public string Birthday
        { 
            get 
            {
                return m_birthday.ToString();
            }
            set
            {
                m_birthday = new Date(value);
            }
        }
        // адрес проживания сотрудника сотрудника не длинее 128 символов
        public string Address
        {
            get
            {
                return m_address;
            }
            set
            {
                if (value.Length > 128)
                {
                    m_address = value.Substring(0, 128);
                }
                m_address = value;
            }
        }
        // отдел в котором работает сотрудник не длинее 32 символов
        public string Group
        {
            get
            {
                return m_group;
            }
            set
            {
                if (value.Length > 32)
                {
                    m_group = value.Substring(0, 32);
                }
                m_group = value;
            }
        }
        // информация сотрудника "о себе " не длинее 256 символов
        public string Info
        {
            get
            {
                return m_info;
            }
            set
            {
                if (value.Length > 256)
                {
                    m_info = value.Substring(0, 256);
                }
                m_info = value;
            }
        }

    }
}
