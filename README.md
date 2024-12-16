Не стал спавнить объекты,их оч мало

Не успел везде сделать общение через ивенты,только между контролерами

Комменты писал тут, не люблю когда в коде куча лишнего

Забуксовал на "При наведении на один из предметов ( все так же, не отпуская ЛКМ) и отпустив на нем ЛКМ, предмет "достается" из "рюкзака"." СДелал как получилось, времени уже не было

Можно было еще отрефакторить, было бы вообще хорошо, но увы, мало времени, поздно сел,думал,что быстро сделаю)

Хорошего дня

# GameHandler

Этот скрипт управляет взаимодействием с инвентарем в игре, включая логику перетаскивания объектов, добавления и удаления их в рюкзак, а также отправки данных на сервер.

## Описание

1. **Инициализация:** При старте скрипта инициализируются все компоненты игры, включая рюкзак, элементы инвентаря и перетаскиваемые объекты.
2. **Перетаскивание объектов:** Когда объект добавляется в рюкзак или извлекается из него, его состояние обновляется, и происходит соответствующая отправка данных на сервер.
3. **Обработчик событий:** Каждый раз, когда объект присоединяется или отсоединяется от рюкзака, отправляется POST-запрос на сервер с указанием события.
4. **Рюкзак и меню рюкзака:** Обработка скрытия и отображения меню рюкзака, а также взаимодействие с моделями и представлениями рюкзака.

## Описание методов

### `Start`

Инициализирует все компоненты, подписываясь на события добавления и удаления объектов в рюкзак.

### `InitDragAndDrops`

Этот метод инициализирует элементы для перетаскивания объектов, создавая модели для каждого объекта, который может быть перетащен.

### `InitBackpack`

Метод инициализирует рюкзак и его отображение, связывая контроллер с моделью рюкзака.

### `InitItem`

Инициализация элементов инвентаря и их взаимодействие с контроллером и представлениями.

### `InitBackpackMenu`

Инициализация меню рюкзака и его взаимодействие с отображением и моделями объектов.

### `ObjectAttachedToBackpack`

Этот метод вызывается, когда объект добавляется в рюкзак. Он замораживает физику объекта (если нужно) и отправляет событие на сервер.

### `ObjectDetachedToBackpack`

Этот метод вызывается, когда объект извлекается из рюкзака. Он "размораживает" объект, позволяя ему двигаться в физическом мире.

### `SendPostRequest`

Метод отправляет POST-запрос на сервер с данными о состоянии объекта, который был добавлен или извлечен из рюкзака.

---

# Drag and Drop Controller

Этот скрипт управляет логикой перетаскивания объектов в Unity, позволяя пользователю взаимодействовать с объектами через мышь.

## Описание

1. **Инициализация:** подключает модели и представления для объектов, которые могут быть перетащены.
2. **Выбор объекта:** при нажатии на объект мышью происходит его выбор и начало перетаскивания.
3. **Перетаскивание:** когда объект выбран, его положение обновляется в зависимости от положения мыши.
4. **Отмена выбора:** при отпускании кнопки мыши объект перестает перетаскиваться, и его выделение снимается.

## Описание методов

### `Init`

Этот метод инициализирует контроллер, подключая модели и представления объектов, которые могут быть перетаскиваемыми. Он устанавливает список моделей и представлений для перетаскиваемых объектов.

### `Update`

В этом методе происходит обработка ввода пользователя. В зависимости от того, была ли нажата кнопка мыши или удерживается ли она, происходит выбор объекта для перетаскивания или его перемещение. Также при отпускании кнопки мыши объект перестает перетаскиваться.

### `TrySelectObject`

Метод пытается выбрать объект, на который нажал пользователь. Если объект был выбран, его координаты экрана преобразуются в мировые координаты, и начинается процесс перетаскивания. При этом объект получает подсветку, чтобы показать, что он выбран. Если объект является элементом типа `ItemView`, его Rigidbody размораживается, чтобы позволить перетаскивание.

### `DragSelectedObject`

Этот метод перемещает выбранный объект в соответствии с положением мыши, обновляя его позицию в мировых координатах. Это позволяет объекту следовать за курсором при перетаскивании.

### `DeselectObject`

Метод отменяет выбор объекта. Когда пользователь отпускает кнопку мыши, объект перестает перетаскиваться, и его выделение снимается.

### `GetMouseWorldPosition`

Этот метод преобразует координаты мыши с экрана в мировые координаты, что позволяет точно расположить перетаскиваемый объект в 3D-пространстве.

---

# Описание работы с инвентарем

В этом проекте используется система перетаскивания объектов, которая позволяет пользователю перемещать предметы в рюкзаке и на игровом поле. Все события, такие как добавление и удаление объектов из рюкзака, фиксируются и отправляются на сервер для синхронизации с внешними системами.

### Пример использования

1. При нажатии на объект мышью происходит его выбор для перетаскивания.
2. Перетаскиваемый объект следит за движением мыши.
3. При отпускании кнопки мыши объект фиксируется в новом месте или отменяется операция перетаскивания.

### Важные моменты

- Каждый объект может быть перемещен только в пределах определенной области, на которую он может быть добавлен (например, в рюкзак).
- Система отправляет запросы на сервер для отслеживания событий добавления и удаления объектов.
- Инвентарь и меню рюкзака можно скрывать и отображать по необходимости.
