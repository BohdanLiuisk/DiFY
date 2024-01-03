/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,scss,ts}"
  ],
  important: false,
  theme: {
    extend: {
      colors: {
        'surface-border': 'var(--surface-border)',
        'surface-card': 'var(--surface-card)',
        'surface-hover': 'var(--surface-hover)',
        'surface-overlay': 'var(--surface-overlay)',
        'surface-100': 'var(--surface-100)',
        'surface-200': 'var(--surface-200)',
        'primary-color': 'var(--primary-color)',
        'blue-200': 'var(--blue-200)',
        'red-500': 'var(--red-500)',
        'green-500': 'var(--green-500)'
      },
      boxShadow: {
        'df-input-shadow': 'var(--df-input-shadow)'
      },
      fontSize: {
        'inherit': 'inherit'
      },
      fontFamily: {
        'inherit': 'inherit'
      }
    },
  },
  plugins: [],
}
