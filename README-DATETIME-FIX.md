# การแก้ไขปัญหา DateTime ใน PostgreSQL

## ปัญหาที่พบ

```
System.ArgumentException: Cannot write DateTime with Kind=Unspecified to PostgreSQL type 'timestamp with time zone', only UTC is supported.
```

## สาเหตุ

PostgreSQL ต้องการ DateTime ที่เป็น UTC เท่านั้น แต่ ASP.NET Core ส่ง DateTime ที่มี Kind=Unspecified

## การแก้ไข

### 1. แก้ไข ApplicationDbContext

เพิ่มการแปลง DateTime เป็น UTC ใน `SaveChanges()` และ `SaveChangesAsync()`:

```csharp
public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
{
    // แปลง DateTime เป็น UTC ก่อนบันทึก
    var entries = ChangeTracker.Entries()
        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

    foreach (var entry in entries)
    {
        foreach (var property in entry.Properties)
        {
            if (property.Metadata.ClrType == typeof(DateTime))
            {
                if (property.CurrentValue != null)
                {
                    var dateTime = (DateTime)property.CurrentValue;
                    if (dateTime.Kind == DateTimeKind.Unspecified)
                    {
                        property.CurrentValue = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                    }
                }
            }
        }
    }

    return await base.SaveChangesAsync(cancellationToken);
}
```

### 2. แก้ไข BooksController

เพิ่มการแปลง DateTime ใน Create method:

```csharp
// แปลง DateTime เป็น UTC
if (book.PublishedDate.Kind == DateTimeKind.Unspecified)
{
    book.PublishedDate = DateTime.SpecifyKind(book.PublishedDate, DateTimeKind.Utc);
}
```

### 3. แก้ไข Program.cs

แปลง DateTime ในข้อมูลตัวอย่าง:

```csharp
PublishedDate = DateTime.SpecifyKind(DateTime.Now.AddYears(-1), DateTimeKind.Utc)
```

## การทดสอบ

หลังจาก Deploy ใหม่:

1. **ทดสอบการเพิ่มหนังสือ** - ควรไม่เกิด DateTime error
2. **ตรวจสอบ Logs** - ไม่ควรมี DateTime error
3. **ทดสอบการแสดงข้อมูล** - วันที่ควรแสดงถูกต้อง

## หมายเหตุ

- การแก้ไขนี้จะแปลง DateTime ทั้งหมดเป็น UTC ก่อนบันทึก
- ข้อมูลที่แสดงจะยังคงเป็นวันที่ที่ถูกต้อง
- การแก้ไขนี้จะใช้กับทั้งการเพิ่มและแก้ไขข้อมูล 