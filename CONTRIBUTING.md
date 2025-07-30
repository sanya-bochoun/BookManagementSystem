# 🤝 Contributing to Book Management System

Thank you for your interest in contributing to the Book Management System project!

## 📋 Getting Started

### System Requirements
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code
- Git
- PostgreSQL (for Production)
- SQL Server LocalDB (for Development)

### Setting up Development Environment

1. **Clone Repository**
   ```bash
   git clone https://github.com/your-username/book-management-system.git
   cd book-management-system
   ```

2. **Install Dependencies**
   ```bash
   dotnet restore
   ```

3. **Setup Database**
   ```bash
   dotnet ef database update
   ```

4. **Run Application**
   ```bash
   dotnet run
   ```

## 🔧 Development

### Coding Standards

#### C# Coding Standards
- Use **PascalCase** for class names, method names, properties
- Use **camelCase** for local variables, parameters
- Use **UPPER_CASE** for constants
- Add XML documentation for public methods

```csharp
/// <summary>
/// Creates a new book
/// </summary>
/// <param name="book">Book data</param>
/// <returns>Creation result</returns>
public async Task<IActionResult> Create(Book book)
{
    // Implementation
}
```

#### Razor Views
- Use **kebab-case** for view names
- Use **PascalCase** for model properties
- Add comments for complex logic

#### CSS/JavaScript
- Use **kebab-case** for CSS classes
- Use **camelCase** for JavaScript variables
- Use **PascalCase** for JavaScript functions

### Git Workflow

1. **Create Feature Branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```

2. **Commit Messages**
   - Use present tense: "Add feature" not "Added feature"
   - Use imperative mood: "Move cursor to..." not "Moves cursor to..."
   - Limit first line to 50 characters
   - Add details in subsequent lines

   ```bash
   git commit -m "Add user authentication feature

   - Implement login/logout functionality
   - Add user roles and permissions
   - Include password reset feature"
   ```

3. **Push and Pull Request**
   ```bash
   git push origin feature/your-feature-name
   ```

### Testing

#### Unit Tests
- Create test files in `Tests/` folder
- Use naming convention: `ClassNameTests.cs`
- Cover business logic at least 80%

```csharp
[Test]
public void CreateBook_WithValidData_ShouldReturnSuccess()
{
    // Arrange
    var book = new Book { Title = "Test Book", Author = "Test Author" };
    
    // Act
    var result = _bookService.CreateBook(book);
    
    // Assert
    Assert.IsTrue(result.IsSuccess);
}
```

#### Integration Tests
- Test API endpoints
- Test database operations
- Test external service integrations

## 🐛 Reporting Bugs

### Bug Report Template

```markdown
**Bug Description**
[Describe the issue that occurred]

**Steps to Reproduce**
1. Go to page...
2. Click on...
3. See error...

**Expected Behavior**
[Describe what should happen]

**Actual Behavior**
[Describe what actually happened]

**Environment**
- OS: [Windows/macOS/Linux]
- Browser: [Chrome/Firefox/Safari]
- .NET Version: [8.0]
- Database: [SQL Server/PostgreSQL]

**Screenshots**
[If available, attach screenshots]

**Additional Information**
[Any additional relevant information]
```

## ✨ Feature Requests

### Feature Request Template

```markdown
**Feature Description**
[Describe the feature you want]

**Use Case**
[Describe the use case]

**Proposed Solution**
[Describe the proposed solution]

**Alternative Solutions**
[If any, alternative solutions]

**Additional Information**
[Any additional information]
```

## 📝 Pull Request Guidelines

### PR Template

```markdown
## Description
[Describe the changes]

## Type of Change
- [ ] Bug fix (non-breaking change which fixes an issue)
- [ ] New feature (non-breaking change which adds functionality)
- [ ] Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] Documentation update

## Testing
- [ ] Unit tests pass
- [ ] Integration tests pass
- [ ] Manual testing completed

## Checklist
- [ ] Code follows the style guidelines
- [ ] Self-review completed
- [ ] Code is commented where necessary
- [ ] Documentation updated
- [ ] No breaking changes (or documented if necessary)

## Screenshots
[If UI changes, attach screenshots]
```

### Review Process

1. **Code Review**
   - Check code quality
   - Check security issues
   - Check performance impact

2. **Testing**
   - Check unit tests
   - Check integration tests
   - Check manual testing

3. **Documentation**
   - Update README if necessary
   - Update API documentation
   - Update comments in code

## 🏷️ Labels

### Issue Labels
- `bug` - Issues that need to be fixed
- `enhancement` - Feature improvements
- `feature` - New features
- `documentation` - Documentation updates
- `good first issue` - Good for beginners
- `help wanted` - Need help

### PR Labels
- `ready for review` - Ready for review
- `work in progress` - Work in progress
- `blocked` - Blocked
- `breaking change` - Breaking changes

## 🎯 Areas for Contribution

### High Priority
- [ ] Add unit tests
- [ ] Improve error handling
- [ ] Add logging
- [ ] Improve performance

### Medium Priority
- [ ] Add new features
- [ ] Improve UI/UX
- [ ] Add documentation
- [ ] Refactor code

### Low Priority
- [ ] Add examples
- [ ] Improve comments
- [ ] Add translations

## 📞 Contact

If you have questions or need help:

- **Email**: your-email@example.com
- **GitHub Issues**: [Create Issue](https://github.com/your-username/book-management-system/issues)
- **Discussions**: [GitHub Discussions](https://github.com/your-username/book-management-system/discussions)

## 🙏 Thank You

Thank you for contributing to the Book Management System!

Every contribution helps make this project better and more useful to the community 🚀 