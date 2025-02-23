import { ThemeProvider, createTheme } from '@mui/material/styles';

const darkTheme = createTheme({
    palette: {
      mode: 'dark',
      primary: {
        main: '#90CAF9', // Light blue for primary elements
      },
      secondary: {
        main: '#F48FB1', // Pinkish-red for secondary elements
      },
      background: {
        default: '#1E1E2F', // Dark background
        paper: '#2A2A3C', // Slightly lighter dark for cards/containers
      },
      text: {
        primary: '#EAEAEA', // White text
        secondary: '#B0BEC5', // Greyed-out text
      },
      action: {
        hover: '#404060', // Hover effect color
        selected: '#3A3A5C', // Selection color
      },
      divider: '#3A3A5C', // Subtle dividers
    },
    typography: {
      fontFamily: `'Inter', 'Roboto', 'Arial', sans-serif`, // Clean modern fonts
      fontSize: 14,
      button: {
        textTransform: 'none', // Keep button text normal case
      },
    },
    components: {
      MuiDataGrid: {
        styleOverrides: {
          root: {
            backgroundColor: '#1E1E2F', // Background of DataGrid
            color: '#EAEAEA', // Text color
          },
          columnHeaders: {
            backgroundColor: '#2A2A3C', // Header row background
            color: '#FFFFFF', // Header text color
          },
          row: {
            '&:nth-of-type(odd)': {
              backgroundColor: '#252538', // Odd row background
            },
            '&:nth-of-type(even)': {
              backgroundColor: '#303048', // Even row background
            },
            '&:hover': {
              backgroundColor: '#404060', // Hover color
            },
          },
          footerContainer: {
            backgroundColor: '#1E1E2F', // Footer background
            color: '#FFFFFF', // Footer text color
          },
          checkbox: {
            color: 'white', // White checkboxes
            '&.Mui-checked': {
              color: 'limegreen', // Checked checkboxes in green
            },
          },
        },
      },
      MuiButton: {
        styleOverrides: {
          root: {
            borderRadius: '8px', // Rounded buttons
            fontWeight: 700,
          },
        },
      },
      MuiPaper: {
        styleOverrides: {
          root: {
            backgroundColor: '#2A2A3C', // Card-like elements
            color: '#EAEAEA',
            padding: '20px',
          },
        },
      },
    },
  });
  

  export default darkTheme;