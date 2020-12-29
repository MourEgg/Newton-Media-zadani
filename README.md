# Newton Media zadáni

- vytvořit .NET C# Web API projekt, doporučuji v .NET Core nebo .NET Framework
- vhodně do kódu zakomponovat následující číselník značek automobilů, reprezentovaný jako JSON soubor

[{"Id":0,"BrandName":"BMW"},{"Id":1,"BrandName":"Audi"},{"Id":2,"BrandName":"Mercedes"},{"Id":3,"BrandName":"Skoda"},{"Id":4,"BrandName":"Fiat"},{"Id":5,"BrandName":"Renault"},{"Id":6,"BrandName":"Lexus"},{"Id":7,"BrandName":"Ferrari"},{"Id":8,"BrandName":"Porsche"},{"Id":9,"BrandName":"Kia"}]

- vytvořit jeden ApiController s jednou API metodou typu HTTP POST, která přijme záznamy o řidičích ve formátu JSON (ukázka dat je v příloze mailu) a vytáhne z nich následující informace:

1) průměrný věk řidičů jako jedno celé číslo
2) počet řidičů podle roku narození (pro každý rok narození v datech vypsat kolik řidičů se v tomto roce narodilo)
3) pokud mají více než dva řidiči stejné příjmení, vypsat o jaké příjmení jde a vypsat ID řidičů s tímto příjmením
4) názvy tří nejčastějších automobilových značek
5) pro každou značku automobilu vypište průměrné staří vozu
6) všechny typy motorů (engineType) a kolik procent řidičů tento motor má
7) ID všech modrookých řidičů, kteří mají alespoň jedno auto s hybridním nebo elektrickým motorem
8) ID řidičů, kteří mají více než jedno auto a všechna auta mají stejný typ motoru

a tyto informace jako odpověď vrátí ve vhodné struktuře a ve formátu JSON zpět.

- pozor na to, ze brandId může odkazovat na neexistující značku - tyto záznamy vypište vhodným způsobem do výsledku API metody jako "varování" pro uživatele, že zadal neznámou značku auta.
