﻿The application receives the current weather in Vilnius from the api.openweathermap.org, then shows it in console and store in the json file.
The main reason for creating this app is learning

// Below will be a report of what was learned, and it will be in Russian. This file is mainly for personal usage

Второй отчет:

В течение недели изучал следующее:
1. Юнит тестирование и moq нугет, добавил юнит тесты в апп использую xUnit
2. Прочитал про различные виды тестирования, особое внимание уделил интеграционным тестам.
3. Ознакомился с HTTP Polly, прочитал про политики и добавил в апп
4. Ознакомился с Branching strategy, прочитал про виды и Git Flow
5. Начал использовать команды Git и прочитал про некоторые из них в документации 

Кроме всего вышеперечисленного переодически переходил к прочтению дополнительного материала, такого как Clean Code (с большего заметки в статьях интернета по клинкоду и фановые тесты с пояснениями). 
Nlog - слегка поправил конфигурационный файл и разбирался как оно работает.
Отправил запрос из постмана на апи, посмотрел результат.

Первый отчет:

Чему я научился и с чем ознакомился:
1.	Узнал, что значит API и получал через него информацию
2.	Понял, что значит DI и смог его настроить
3.	Ознакомился с SOLID принципами и пытался их применять
4.	Старался следовать принципам DRY KISS YAGNI
5.	Прочитал почему лучше применять HttpClientFactory вместо HttpClient и поменял в коде
6.	Узнал про StatusCode ответов от сервера и добавил проверки на некоторые из них
7.	Ознакомился с n-леерной архитектурой и пробовал ее применить
8.	Почитал про Domain-Driven Design, но лучше разбираться с этим на примере
9.	Прочитал про логирование и добавил логгер 
10.	Прочитал про CICD и сделал (ну почти) что-то похожее локально c CLI и powershell 

В целом, даже на таком маленьком аппе получил много полезного опыта, практических и теоретических знаний. 
В процессе создания и развития аппа мне многое было в новинку, что-то я вообще не знал, что-то только теоретически, например, сериализацию и десериализацию до этого никогда не применял и в принципе не работал с JSON. 
Пришло некоторые понимание как работать с appsetting, а как заставить тянуть информацию из файла по его названию установкой параметра в пропертис было небольшим шоком.  
Также, в процессе создания аппа, учился пользоваться теми вещами, которыми якобы умел, один раз случайно откатил полностью изменения сделанные локально пул реквестом из гитхаба и потом переделывал все сделанное за день 😊.
В планах перечитать статьи по IHttpClientFactory, так как пробегался по верхам с целью быстрее запкустить апп, а также еще раз глянуть статьи по DDD. 