# File template

Il file .json usato come template è composto come segue:
- sezione main: OBBLIGATORIA
- sezioni ausiliarie: OPZIONALI

```
{
    "main": {
        ...json che viene randomizzato e restituito come risultato...
    },

    
    "&aux_1": {
	...
    },
    "&aux_2": {
	...
    }
}
```

# Keyword di tipo e argomenti

Tipi attualmente supportati:
- $int
- $string
- $double
- $date

Sintassi nel template:

"$tipo min..max"

Il significato di min e max varia in base al tipo:

$int: valore minimo e massimo
$string:  numero di caratteri minimi e massimi
$double: valore minimo e massimo
$date: data minima e massima

# Token &

Un valore che inizia con il token & seguito dal nome di una sezione dichiarata nel template json sostituisce il valore con quella sezione.
Tutte le proprietà delle sottosezioni possono essere a loro volta generate casualmente. Può essere usato insieme alla keyword $repeat se in una lista.

Es

{
	"main": {
		"name": "mario",
		"surname": "biondi"
		"indirizzo": "&address"
	},
	"&address": {
		"via": ...		// può essere generata
		"civico": ...	// può essere generata
		"CAP": ...		// può essere generata
	}
}

{
	"main": {
		"name": "mario",
		"surname": "biondi"
		"residenze": [
			"&address $repeat 4"
		]
	},
	"&address": {
		"via": ...		// può essere generata
		"civico": ...	// può essere generata
		"CAP": ...		// può essere generata
	}
}

# Keyword $repeat

- Se usata in un array, permette di ripetere un certo valore generato n volte o in un range min..max

Es $int 0..100 $repeat 10 => crea un array con 10 interi da 0 a 100
Es $int 0..100 $repeat 1..5 => crea un array con min 1 max 5 interi da 0 a 100

- Se usata nel nome di una proprietà la ripete n volte o in un range min..max

Es 

"$string 10 $repeat 3": {
	"name": "mario",
	"surname": "biondi"
}

=> crea 3 oggetti con una stringa casuale come nome con valore l'oggetto { "name": "mario", "surname": "biondi" }. 
Eventuali collisioni coi nomi sono gestite automaticamente, con unico vincolo il range che deve essere minore dell'intervallo di generazione.
Le proprietà annidate possono essere a loro volta casuali.

# Keyword $seq

Usata come valore di una proprietà un numero di sequenza che parte da 0 viene assegnato alla proprietà.

Es

"people": [
	"&person $repeat 3"
]

"&person":{
	"seqNum": "$seq",
	"name": "mario"
}

produce come output:

"people": [
	{
		"seqNum": 0,
		"name": "mario"
	},
	{
		"seqNum": 1,
		"name": "mario"
	},
	{
		"seqNum": 2,
		"name": "mario"
	},
]
