# Unreal Engine 4 save game converter

将 ++UE4+Release-4.18 GVAS 二进制格式的文件，解析为json，以便于分析

```
>GvasConverter.exe "SaveData2.sav"
```

Bakc convertion is theoretically possible, but is not implemented.

Due to limitations of how UE4 serializes the data, some data types might be missing, and might fail deserialization for some games.
For example, I know for a fact that there's at least a Set collection type, and a lot of less-frequently used primitive types (non-4 byte ints, double, etc).
