import js from '@eslint/js';
import globals from 'globals';
import reactHooks from 'eslint-plugin-react-hooks';
import reactRefresh from 'eslint-plugin-react-refresh';
import tseslint from 'typescript-eslint';
import prettier from 'eslint-config-prettier';
import preferArrowFunctions from 'eslint-plugin-prefer-arrow-functions';
import unusedImports from 'eslint-plugin-unused-imports';

export default tseslint.config(
  {
    ignores: [
      'dist',
      'eslint.config.js',
      'vite.config.ts',
      'src/components/ui/**/*',
      '**/_generatedClient/*',
    ],
  },
  {
    extends: [
      js.configs.recommended,
      ...tseslint.configs.recommendedTypeChecked,
    ],
    files: ['**/*.{ts,tsx}'],
    languageOptions: {
      ecmaVersion: 2020,
      globals: globals.browser,
      parserOptions: {
        projectService: true,
        tsconfigRootDir: import.meta.dirname,
      },
    },
    plugins: {
      'react-hooks': reactHooks,
      'react-refresh': reactRefresh,
      prettier,
      'prefer-arrow-functions': preferArrowFunctions,
      'unused-imports': unusedImports,
    },
    rules: {
      ...reactHooks.configs.recommended.rules,
      'react-refresh/only-export-components': [
        'warn',
        { allowConstantExport: true },
      ],
      'no-debugger': 'error',
      'no-console': 'warn',
      'no-shadow': 'error',
      eqeqeq: ['error', 'always'],
      'prefer-const': ['error', { destructuring: 'all' }],
      '@typescript-eslint/no-explicit-any': 'warn',
      '@typescript-eslint/interface-name-prefix': 'off',
      '@typescript-eslint/no-floating-promises': 'off',
      'unused-imports/no-unused-imports': 'error',
      'no-await-in-loop': 'warn',
      '@typescript-eslint/no-unused-vars': 'warn',
      'no-param-reassign': 'warn',
      'lines-between-class-members': 'error',
      'no-trailing-spaces': 'warn',
      'unused-imports/no-unused-vars': [
        'warn',
        {
          vars: 'all',
          varsIgnorePattern: ',^_',
          args: 'after-used',
          argsIgnorePattern: '^_',
        },
      ],
      'prefer-arrow-functions/prefer-arrow-functions': [
        'warn',
        {
          classPropertiesAllowed: true,
          disallowPrototype: false,
          returnStyle: 'unchanged',
          singleReturnOnly: false,
        },
      ],
      'react/react-in-jsx-scope': 'off',
      '@typescript-eslint/no-non-null-assertion': 'off',
    },
  }
);
