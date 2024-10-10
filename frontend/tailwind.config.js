/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: ["class"],
  content: [
    './pages/**/*.{ts,tsx}',
    './components/**/*.{ts,tsx}',
    './app/**/*.{ts,tsx}',
    './src/**/*.{ts,tsx}',
  ],
  theme: {
    container: {
      center: true,
      padding: "2rem",
      screens: {
        "2xl": "1400px",
        xl: "1280px",
        lg: "1024px",
        md: "768px",
        sm: "640px",
      },
    },
    extend: {
      colors: {
        border: "var(--border)", // Zmienna CSS
        input: "var(--input)", // Zmienna CSS
        ring: "var(--ring)", // Zmienna CSS
        background: "var(--background)", // Zmienna CSS
        foreground: "var(--foreground)", // Zmienna CSS
        primary: {
          DEFAULT: "var(--primary)", // Zmienna CSS
          foreground: "var(--primary-foreground)", // Zmienna CSS
        },
        secondary: {
          DEFAULT: "var(--secondary)", // Zmienna CSS
          foreground: "var(--secondary-foreground)", // Zmienna CSS
        },
        destructive: {
          DEFAULT: "var(--destructive)", // Zmienna CSS
          foreground: "var(--destructive-foreground)", // Zmienna CSS
        },
        muted: {
          DEFAULT: "var(--muted)", // Zmienna CSS
          foreground: "var(--muted-foreground)", // Zmienna CSS
        },
        accent: {
          DEFAULT: "var(--accent)", // Zmienna CSS
          foreground: "var(--accent-foreground)", // Zmienna CSS
        },
        popover: {
          DEFAULT: "var(--popover)", // Zmienna CSS
          foreground: "var(--popover-foreground)", // Zmienna CSS
        },
        card: {
          DEFAULT: "var(--card)", // Zmienna CSS
          foreground: "var(--card-foreground)", // Zmienna CSS
        },
        // Admin theme colors
        admin: {
          background: "var(--background)", // Zmienna CSS
          foreground: "var(--foreground)", // Zmienna CSS
          primary: {
            DEFAULT: "var(--primary)", // Zmienna CSS
            foreground: "var(--primary-foreground)", // Zmienna CSS
          },
          secondary: {
            DEFAULT: "var(--secondary)", // Zmienna CSS
            foreground: "var(--secondary-foreground)", // Zmienna CSS
          },
          accent: {
            DEFAULT: "var(--accent)", // Zmienna CSS
            foreground: "var(--accent-foreground)", // Zmienna CSS
          },
          muted: {
            DEFAULT: "var(--muted)", // Zmienna CSS
            foreground: "var(--muted-foreground)", // Zmienna CSS
          },
        },
        // New custom color for central section
        centralSection: {
          foreground: "var(--central-section-foreground)", // Zmienna CSS
          background: "var(--central-section-background)", // Zmienna CSS
        },
      },
      borderRadius: {
        lg: "var(--radius)",
        md: "calc(var(--radius) - 2px)",
        sm: "calc(var(--radius) - 4px)",
      },
      keyframes: {
        "accordion-down": {
          from: { height: 0 },
          to: { height: "var(--radix-accordion-content-height)" },
        },
        "accordion-up": {
          from: { height: "var(--radix-accordion-content-height)" },
          to: { height: 0 },
        },
      },
      animation: {
        "accordion-down": "accordion-down 0.2s ease-out",
        "accordion-up": "accordion-up 0.2s ease-out",
      },
    },
  },
  plugins: [require("tailwindcss-animate")],
}
