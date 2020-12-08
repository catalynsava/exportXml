UPDATE ADRROL SET CNP = nrUI WHERE (CNP="" or cnp is null) AND (TIP=3 OR TIP=4);
UPDATE ADRROL SET TIPEXPLOA="3. c) societate comerciala cu capital majoritar privat (Legea nr. 31/1990)" WHERE  NUME like "* SRL*" AND (TIP=3 OR TIP=4);
UPDATE ADRROL SET TIPEXPLOA="3. c) societate comerciala cu capital majoritar privat (Legea nr. 31/1990)" WHERE  NUME like "* SA*" AND (TIP=3 OR TIP=4);
UPDATE ADRROL SET TIPEXPLOA="3. c) societate comerciala cu capital majoritar privat (Legea nr. 31/1990)" WHERE  NUME like "CAMIN CULTURAL*" AND (TIP=3 OR TIP=4);
UPDATE adrrol SET tipexploa = "3. g) alte institutii publice centrale sau locale" WHERE NUME LIKE "DISPENSAR*" AND (TIP=3 OR TIP=4);
<--DIRECTIA SILVICA
UPDATE adrrol SET tipexploa = "3. f) unitate/ subdiviziune administrativ teritoriala" WHERE NUME LIKE "DIRECTIA SILVICA*" AND (TIP=3 OR TIP=4);
<--SCOALA
UPDATE adrrol SET tipexploa = "i) alte tipuri (asociatie, fundatie, asezamant religios, scoala etc.)" WHERE NUME LIKE "SCOALA*" AND (TIP=3 OR TIP=4);
<--GRADINITA
UPDATE adrrol SET tipexploa = "i) alte tipuri (asociatie, fundatie, asezamant religios, scoala etc.)" WHERE NUME LIKE "GRADINITA*" AND (TIP=3 OR TIP=4);
<--PAROHIA
UPDATE adrrol SET tipexploa = "i) alte tipuri (asociatie, fundatie, asezamant religios, scoala etc.)" WHERE NUME LIKE "PAROHIA*" AND (TIP=3 OR TIP=4);
<--CAMIN CULTURAL
UPDATE adrrol SET tipexploa = "3. g) alte institutii publice centrale sau locale" WHERE NUME LIKE "CAMIN CULTURAL*" AND (TIP=3 OR TIP=4);