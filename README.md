# JSON Template Randomizer
This library allows the creation of fake data using a templating language in order to
reduce time spent writing cumbersome and hard to modify JSON files.

# Template file

The .json file used as a template is composed as follows:
- main section: REQUIRED
- auxiliary sections: OPTIONAL

```json
{
  "main": {
    "...json that is randomized and returned as a result...": "..."
  },

    
  "&auxiliary_1": {
    "...": "..."
  },
  "&auxiliary_2": {
    "...": "..."
  }
}
```

## Type keywords and arguments

Currently supported types:
Integer, Double, String, Date (with offset)

Syntax in the template:

```
"$type min..max args"
```

The meaning of min and max varies by type:

- Integer: minimum and maximum value
- String: minimum and maximum number of characters
- Double: minimum and maximum value
- Date: minimum and maximum date

## Keyword $repeat

When used in an array, allows to repeat a certain generated value the defined number of times or in a range `min..max`.

- `"$int 0..100 $repeat 10"` creates an array with 10 integers from 0 to 100
- `"$int 0..100 $repeat 1..5"` creates an array with 1-5 integers from 0 to 100

If used in property name repeats it the defined number of times or in a range `min..max`.

```json
{
  "$string 10 $repeat 3": {
    "name": "mario",
    "age": 30
  }
}
```

The previous section reates 3 objects with a random string as name with value the object `{ "name": "mario", "age": 30 }`.

Any collisions with the names are handled automatically. Nested properties can themselves be random.

## Token & (auxiliary section reference)

A value starting with token `&` followed by the name of a section declared in the json template replaces the value with that section.
All subsection properties can themselves be randomly generated. Can be used in conjunction with the keyword `$repeat` in a list.

The following template produces the json below:
```json
{
    "main": {
        "name": "mario",
        "surname": "blondes",
        "residences": [
        "&address $repeat 2"
        ]
    },
	
    "&address": {
        "street": "...",
        "civic": "...",	
        "ZIP CODE": "..."	
    }
}
```

```json
{
    "name": "mario",
    "surname": "blondes",
    "residences": [
        {
            "street": "...",		
            "house_number": "...",	
            "zip": "..."
        },
        {
            "street": "...",		
            "house_number": "...",	
            "zip": "..."
        }		
    ]
}

```

## Keyword $seq

Used as the value of a property a sequence number starting with 0 is assigned to the property.

```json
{
  "people": [
    "&person $repeat 3"
  ],
  
"&person":{
  "seqNum": "$seq",
  "name": "mario"
  }
}
```

The above template produces the following output:

```json
{   
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
    }
  ]
}
```
