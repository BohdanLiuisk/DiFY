# DiFY
Just pet project.
## Stack: 
- .NET 8
- Angular 17 (planning to migrate to Vue 3)

## Existing functionality: 
- custom entity creator on backend
- video calls

# Dify Entity

## Overview of creating tables
This documentation describes the configuration for creating tables in a database, using a JSON-based schema. Below is a detailed explanation of the properties required to define tables and columns, along with examples.

## JSON structure example
The following JSON is an example of how to define a table and its columns:

```json
{
  "id": "65a93638-295f-4e3a-81e2-4bb13f27edc7",
  "name": "contact",
  "caption": "Contact",
  "columns": [
    {
      "id": "835540ca-e156-4aab-9136-40e215360b9b",
      "name": "id",
      "caption": "Id",
      "type": 11,
      "isPrimaryKey": true
    },
    {
      "id": "ec787866-d762-4622-8e80-00009fabdbca",
      "name": "name",
      "caption": "Name",
      "type": 16,
      "size": 250,
      "isNullable": false
    },
    // Other columns...
  ]
}
```

## Properties description

### Table properties
- **id**: A unique identifier for the table (UUID format).
- **name**: The name of the table in the database.
- **caption**: A human-readable name for the table, often used in UI.
- **columns**: An array of column definitions that describe the structure of the table.

### Column properties
- **id**: A unique identifier for the column (UUID format).
- **name**: The name of the column in the database.
- **caption**: A human-readable name for the column.
- **type**: Specifies the data type for the column (see **Data Types** section).
- **isPrimaryKey**: Indicates if the column is the primary key (optional, default: `false`).
- **isNullable**: Defines if the column allows null values (optional, default: `true`).
- **size**: Used for character & decimal types to define the maximum length (optional).
- **precision**: Used decimal type to define the scale (optional).
- **referenceEntityId**: If the column is a foreign key, this specifies the referenced entity's ID (optional).

## Data types
The following table describes the available data types for columns:

| Type Code | Data Type       | PostgreSQL Value         | Additional properties               | Behavior description                                                                 |
|-----------|-----------------|--------------------------|-------------------------------------|---------------------------------------------------------------------------------------|
| 8         | Double          | `double precision (float8)` | none                                | Type can be changed to other numeric types.                                           |
| 15        | Single          | `real (float4)`           | none                                | Type can be changed to other numeric types.                                           |
| 10        | Int16           | `smallint`                | none                                | Type can be changed to other numeric types.                                           |
| 11        | Int32           | `integer`                 | none                                | Type can be changed to other numeric types.                                           |
| 12        | Int64           | `bigint`                  | none                                | Type can be changed to other numeric types.                                           |
| 7         | Decimal         | `decimal`                 | `size` (max: 1000, default: 19), `precision` (default: 5, should be less than `size`) | Type can be changed to other numeric types, `size` can be modified but only to greater value, `precision` can be modified to any other value.   |
| 4         | Currency        | `money`                   | none                                | Type can be changed to other numeric types.                                           |
| 3         | Boolean         | `boolean`                 | none                                | Type cannot be changed.                                                               |
| 16        | String          | `text` or `varchar(size)` | `size` - `varchar($size)`               | `size` can be modified but only to greater value and if it was present before (if it wasn’t `text`)                                      |
| 6         | DateTime        | `timestamp`               | none                                | Type cannot be changed.                                                               |
| 27        | DateTimeOffset  | `timestamptz`             | none                                | Type cannot be changed.                                                               |
| 5         | Date            | `date`                    | none                                | Type cannot be changed.                                                               |
| 17        | Time            | `time`                    | none                                | Type cannot be changed.                                                               |
| 1         | Binary          | `bytea`                   | none                                | Type cannot be changed.                                                               |

## Best practices
- **Primary key**: Always define a primary key column to uniquely identify each row.
- **Foreign keys**: Use the `referenceEntityId` to establish relationships between tables.
- **Nullable columns**: Ensure that `isNullable` is set appropriately, depending on the data requirements.
- **Data type changes**: Follow the behavior guidelines to avoid issues when altering column types.

# Dify Entity Query
to be described...