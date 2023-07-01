# WinformPaintedControls

# Мета

Оновити візуальний вигляда елементів Winforms. Для цього потрібно додати можливість:

<ul>
    <li>Змінювати колір границі елемента.
    <li>Змінювати товщину границі елемента
    <li>Змінювати колір всього елемента в залежності від його стану
</ul>

Стандартні елементи не підтримують зміну кольору границі чи його товщину (окрім <b color="blue">Button</b>). Деякі з них також не мають можливості легкої зміни кольору внутріншніх елементів (наприклад <b>ListBox, TreeView</b>). Деякі є самі по собі складними і не мають можливості прямої зміни (<b>ComboBox</b>). Тому вирішено створити набір <b>Controls</b>, що замінюють їх і дозволяють самостійно задавати кольори/зовнішній вигляд.

Було створено:

- [CustomButton](#CustomButton)

- [CustomCheckBox](#CustomCheckBox)

- [CustomComboBox](#CustomComboBox)

- [CustomListBox](#CustomListBox)

- [CustomPanel](#CustomPanel)

- [CustomTextBox](#CustomTextBox)

- [PaintedProgressBar](#PaintedProgressBar)

- [PaintedTreeView](#PaintedTreeView)

# Основа

За основу взята та ідея, що при взаємодії з елементами форми вони переходять у деякі стани. Ці стани можна позначати кольором.

Для визначення станів є статичний клас **ColorStates** (замість нього можна було використати enum, але для уникання конвертацій вирішено використовувати статичні константи), де зберігають значення Default, Press, Hover, Check. 

Для обробки є контейнери ColorContainers, що мають певні кольори для кожного стану і поточний стан, за яким видається поточний колір. Є контейнери, що використовують два стани, три або усі чотири. Для кожного контролу використовуєть по два контейнера  (один - для кольору тексту і границі, другий - для фарбування тла).

Для використання даних контейнерів у дизайнері в Visual Studio є класи **ColorStateConverter**.

Всі Custom-контроли є наслідниками **BorderControl**. Це клас, що наслідується від **Control** і має композитно основний елемент, що відображається. **BorderControl** виступає батьком і малює на собі границю, дочірній елемент розташований в межах границі. Такий підхід дозволяє

1. Малювати границю **всередині** елемента. Тобто, границя виступає частиною даного елемента, що дозволяє зручно маніпулювати положеннями і розмірами елемента.

2. Тримати основний елемент в межах границі. Його розміри чітко визначені і не можуть бути перекриті границею. Вони не заважають один одному. *(Для **Panel** це особливо важливо, бо у випадку, коли дочірній елемент виходить за межі **Panel**, то візуально "малюється під границею". Інші випробувані мної підходи дають ефект малювання над границею)*.

Мінусом даного підходу є те, що багато важливих властивостей залишаються у основного дочірнього елемента, і доступ до них є дещо ускладненим. Основні події також приймає саме цей елемент. Тому у коді для багатьох класів є властивість, що відповідає основному елементу (наприклад, **List** для класу **ListBox**), що дозволяє отримати необхідні події/властивості/методи. Іноді властивості самого класу перевизначені або розкриті для використання методів дочірнього елементу.

# Користувацькі форми

## CustomButton

Унаслідуваний від **BorderControl**, має елемент **Label**. Передбачено кольори Default, Hover, Press для тла і границі. Колір границі і тексту збігаються. При натисканні запускає подію OnClick. Інші події для прив'язки не дієві. Текст завжди по центру.



