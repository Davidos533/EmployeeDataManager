using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmployeeDataManager
{
    // класс для описания даты
    public class Date
    {
        private short m_day;
        private short m_month;
        private short m_year;
        // свойства класса
        public short day 
        {
            get
            {
                return m_day;
            }
            private set
            {
                // если переданное значение входит в диапозон реальных дней месяца
                if (value >= 1 && value <=31)
                { 
                    m_day = value;        // установка значения в свойство
                }
            }
        }
        public short month 
        {
            get
            {
                return m_month;
            }
            private set
            {
                // если переданное значение входит в диапозон реальных номеров месяцев
                if (value >= 1 && value <= 12)
                {
                    m_month = value;      // установка значения в свойство
                }
            }
        }
        public short year
        {
            get
            {
                return m_year;
            }
            private set
            {
                // если переданное значение не является отрицательным
                if (value >= 0)
                {
                    m_year = value;       // установка значения в свойство
                }
            }
        }
        // конструкторы класса
        public Date(short _day, short _month, short _year)
        {
            day = _day;
            month = _month;
            year = _year;
        }
        public Date(string _date)
        {
            // проверка находится ли переданная строка в формате даты
            if(Regex.IsMatch(_date, @"(\d{2}[-\s.]){2}\d{4}"))
            {
                day = short.Parse(_date.Substring(0, 2));
                month= short.Parse(_date.Substring(3, 2));
                year= short.Parse(_date.Substring(6, 4));
            }
        }
        // перегрузка метода получения строки из типа
        public override string ToString()
        {
            // создание объекта StrinBuilder для работы со строками
            StringBuilder date = new StringBuilder();

            DateAppend(day.ToString(), date, 2);
            date.Append("-");
            DateAppend(month.ToString(), date, 2);
            date.Append("-");
            DateAppend(year.ToString(), date, 4);

            return date.ToString();
        }
        // метод для добавления нулей в начало числа, которое в дальнейшем будет использоваться как строка
        private StringBuilder DateAppend(string number, StringBuilder sb,int normalLength)
        {
            for (int i = number.Length; i < normalLength; i++)
            {
                sb.Append("0");
            }
            sb.Append(number);
            return sb;
        }
    }
}
