# Pochta (Mail.exe)
Приложение реализующее **вход в аккаунт** домена Gmail.com и **отправление сообщения(-ий) пользователю(-ям)**, 
а также **сохранять** данное сообщение как **черновик** (на компьютере пользователя).

Приложение:
<br>1. Окно входа в аккаунт. Интерфейс:

  - Поле ввода *адреса* почты (домена Gmail.com)
  - Поле ввода *пароля* от аккаунта почты (введенные символы не видно)
  - Кнопка *прислать* числовой код - с **моей** почты на введенный адрес приходит сообщение, содержащее уникальный код из 6 цифр
  - Поле *ввода кода*
  - Кнопка *проверить* код - проверяет совпадает ли введенный код с тем, что был отправлен на почту
  - Кнопка *войти* в аккаунт - разблокируется, когда код подтверждается
<br>
2. Окно создания сообщения. Интерфейс:

  - Поле "От кого" (отправителя) - не редактируется, при открытии окна заполняется введенной почтой
  - Поле "Кому" - содержит получателя сообщения
  - Кнопка "Добавить адрес в рассылку" - адрес, введенный в поле "Кому" будет записываться в листбокс с рассылкой
  - Листбокс с рассылкой (несколькими получателями) - содержит список адресов
  - Кнопка "Удалить адрес" - удаляет адрес эл.почты из листбокса
  'Эти элементы становятся видимыми, когда добавляется хотя бы один адрес в рассылку, и исчезают, когда листбокс становится пустым'
  - Листбокс с вложениями (хранит в себе список путей до вложений) - чтобы добавить вложение, перетаскиваем файл из проводника. 
  Если файл, разрешенного расширения (при добавлении файла с неправильным расширением будет выводиться ошибка), 
  то он добавится в список. Нельзя добавлять во вложения папки.
  - Поле "Тема" - содержит тему сообщения (может быть пустым)
  - Поле "Текст" - содержимое сообщения
  - Кнопка "Отправить сообщение" - отправляет сообщение 
  - Кнопка "Добавить в черновик" - создает в текущей папке (где запускается приложение) директорию "chernoviki" (если нету), куда записывает файл-черновик с расширением ".chern"
  
  
<br>
3. Нюансы использования приложения:

  - Используется устаревший интернет-протокол SMTP (стоит заменить его на MailKit)
  - Проблема сохранения путей к вложениям в черновик 
  - Проблема аутентификации входа в аккаунт - по идее, числовой код должен как-то подтверждать, что 
  пароль введён верно, но это никак не мешает пользователю ввести неверный пароль (**FIX IT**)
  - По какой-то причине, при открытии черновика с рассылкой (несколькими получателями), сообщение будет отправляться как одному получателю, хотя их указано несколько
